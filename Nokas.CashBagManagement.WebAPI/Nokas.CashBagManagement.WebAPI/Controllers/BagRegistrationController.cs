using System.Text.Json;
using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nokas.CashBagManagement.WebAPI.Helpers;
using Nokas.CashBagManagement.WebAPI.Models;
using Nokas.CashBagManagement.WebAPI.Repository;

namespace Nokas.CashBagManagement.WebAPI.Controllers
{
    [Authorize]
    [Route("api/cashbag/register")]
    [ApiController]
    public class BagRegistrationController : ControllerBase
    {
        private readonly ILogger<BagRegistrationController> _logger;
        private readonly IBagRegistrationRepo _bagRegistrationRepo;
        private readonly IMapper _mapper;

        public BagRegistrationController(
            ILogger<BagRegistrationController> logger,
            IBagRegistrationRepo bagRegistrationRepo,
            IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _bagRegistrationRepo = bagRegistrationRepo ?? throw new ArgumentNullException(nameof(bagRegistrationRepo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        private string GetCorrelationId() =>
            HttpContext?.Items["CorrelationId"]?.ToString() ?? "N/A";

        [HttpGet("{bagNumber}", Name = "GetBagRegistration")]
        public async Task<ActionResult<BagRegistrationRequest>> GetBagRegistrationByNumber(string bagNumber)
        {
            var correlationId = GetCorrelationId();
            foreach (var claim in User.Claims)
            {
                Console.WriteLine($"Claim: {claim.Type} = {claim.Value}");
            }
            var clientId = ClaimsHelper.GetClientId(User);

            if (string.IsNullOrEmpty(clientId))
            {
                _logger.LogWarning("CorrelationId: {CorrelationId} - Missing clientId.", correlationId);
                return Unauthorized();
            }


            var bagRegistration = await _bagRegistrationRepo.GetBagByNumberForClientAsync(bagNumber, clientId);
            if (bagRegistration == null)
            {
                return NotFound($"No bag found with the BagNumber: {bagNumber} for this client with Id: {clientId}.");
            }

            return Ok(bagRegistration);
        }

        [HttpPost]
        public async Task<ActionResult<BagRegistrationResponse>> CreateBagRegistration(BagRegistrationRequestForCreation bagRequest)
        {
            var correlationId = GetCorrelationId();
            var clientId = ClaimsHelper.GetClientId(User);

            if (bagRequest == null || !ModelState.IsValid)
            {
                _logger.LogError("CorrelationId: {CorrelationId} - Invalid model or bad request for {RequestPath}.", correlationId, Request.Path);
                return BadRequest("Invalid request data");
            }

            var mappedBagRegistration = _mapper.Map<BagRegistration>(bagRequest.BagRegistration);

            var newBagRegistrationRequest = new BagRegistrationRequest
            {
                Id = bagRequest.BagRegistration?.BagNumber,
                BagRegistration = mappedBagRegistration,
                CacheDbRegistrationId = bagRequest.CacheDbRegistrationId,
                RegistrationType = bagRequest.RegistrationType,
                CustomerCountry = bagRequest.CustomerCountry,
                Status = "In-Progress", // Default status
                ClientId = clientId // Set the clientId from claims
            };

            await _bagRegistrationRepo.CreateBagRegistration(newBagRegistrationRequest);

            var response = new BagRegistrationResponse
            {
                BagNumber = bagRequest.BagRegistration?.BagNumber,
                Status = "Success",
                Message = "Bag registration created successfully.",
                CorrelationId = correlationId
            };

            _logger.LogInformation("CorrelationId: {CorrelationId} - Bag registration created successfully.", correlationId);

            return Ok(response);
        }
    }
}
