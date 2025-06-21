using System.Text;
using Azure.Storage.Blobs;

namespace Nokas.CashBagManagement.WebAPI.Services
{
    public class BlobArchiveService : IBlobArchiveService
    {
        private readonly BlobContainerClient _containerClient;

        public BlobArchiveService(IConfiguration configuration)
        {
            var connectionString = configuration["BlobStorage:ConnectionString"];
            var containerName = configuration["BlobStorage:ContainerName"];
            _containerClient = new BlobContainerClient(connectionString, containerName);
            _containerClient.CreateIfNotExists();
        }

        public async Task ArchivePayloadAsync(string blobName, string content)
        {
            var blobClient = _containerClient.GetBlobClient(blobName);
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            await blobClient.UploadAsync(stream, overwrite: true);
        }
    }
}
