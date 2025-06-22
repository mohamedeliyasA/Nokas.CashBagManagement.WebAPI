using System.Text.Json;
using AutoMapper;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nokas.CashBagManagement.WebAPI.Models;

namespace Nokas.CashBagManagement.WebAPI.Repository
{
    public class CosmosBagRegistrationRepo : IBagRegistrationRepo
    {
        private readonly Container _container;
        private readonly IMapper _mapper;
        private readonly ILogger<CosmosBagRegistrationRepo> _logger;

        public CosmosBagRegistrationRepo(CosmosClient cosmosClient,
                                         IConfiguration configuration,
                                         IMapper mapper,
                                         ILogger<CosmosBagRegistrationRepo> logger)
        {
            var databaseName = configuration["CosmosDb:DatabaseName"];
            var containerName = configuration["CosmosDb:ContainerName"];
            _container = cosmosClient.GetContainer(databaseName, containerName);

            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<BagRegistrationRequest?> GetBagByNumberForClientAsync(string bagNumber, string clientId)
        {
            try
            {
                var query = new QueryDefinition(
                             "SELECT * FROM c WHERE c.clientId = @clientId AND c.BagRegistration.BagNumber = @bagNumber")
                                .WithParameter("@clientId", clientId)
                                .WithParameter("@bagNumber", bagNumber);

                var iterator = _container.GetItemQueryIterator<BagRegistrationRequest>(
                    query,
                    requestOptions: new QueryRequestOptions
                    {
                        PartitionKey = new PartitionKey(clientId)
                    });

                var page = await iterator.ReadNextAsync();
                return page.Resource.FirstOrDefault();

            }
            catch (CosmosException ex)
            {
                _logger.LogError(ex, "Cosmos query failed for BagNumber: {BagNumber}, ClientId: {ClientId}", bagNumber, clientId);
                throw;
            }
        }

      
        public async Task<BagRegistrationRequest> CreateBagRegistration(BagRegistrationRequest bagRegistrationRequest)
        {
            if (bagRegistrationRequest == null)
                throw new ArgumentNullException(nameof(bagRegistrationRequest));

            try
            {
 
                bagRegistrationRequest.BagRegistration.RegistrationStatus ??= "In-Progress";

                var clientId = bagRegistrationRequest.ClientId;

                var response = await _container.CreateItemAsync(bagRegistrationRequest, new PartitionKey(clientId));

                return response.Resource;
            }
            catch (CosmosException ex)
            {
                _logger.LogError(ex, "Failed to create bag registration for BagNumber: {BagNumber}", bagRegistrationRequest.BagRegistration?.BagNumber);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while creating bag registration.");
                throw;
            }
        }
    }
}
