using System.Text.Json;
using AutoMapper;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nokas.CashBagManagement.WebAPI.Entities;
using Nokas.CashBagManagement.WebAPI.Helpers;
using Nokas.CashBagManagement.WebAPI.Middleware;
using Nokas.CashBagManagement.WebAPI.Models;
using Nokas.CashBagManagement.WebAPI.Repository;
using Nokas.CashBagManagement.WebAPI.Services;

namespace Nokas.CashBagManagement.WebAPI.Controllers
{
    /// <summary>
    /// Handles operations related to cash bag registration.
    /// </summary>
    [Authorize]
    [Route("api/cashbag/")]
    [ApiController]
    [Produces("application/json", "application/xml")]
    public class BagRegistrationController : ControllerBase
    {
        private readonly ILogger<BagRegistrationController> _logger;
        private readonly IBagRegistrationRepo _bagRegistrationRepo;
        private readonly IBlobArchiveService _blobArchiveService;
        private readonly IServiceBusSender _serviceBusSender;
        private readonly IMapper _mapper;

        public BagRegistrationController(
            ILogger<BagRegistrationController> logger,
            IBagRegistrationRepo bagRegistrationRepo,
            IBlobArchiveService blobArchiveService,
            IServiceBusSender serviceBusSender,
            IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _bagRegistrationRepo = bagRegistrationRepo ?? throw new ArgumentNullException(nameof(bagRegistrationRepo));
            _blobArchiveService = blobArchiveService ?? throw new ArgumentNullException(nameof(blobArchiveService));
            _serviceBusSender = serviceBusSender ?? throw new ArgumentNullException(nameof(serviceBusSender));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        private string GetCorrelationId() =>
            HttpContext?.Items["CorrelationId"]?.ToString() ?? "N/A";

        [HttpGet("{bagNumber}", Name = "GetBagRegistration")]
        [ProducesResponseType(typeof(BagRegSummaryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BagRegistrationRequestForCreation>> GetBagRegistrationByNumber(string bagNumber)
        {
            var requestCorrelationId = GetCorrelationId();
            var clientId = ClaimsHelper.GetClientId(User);

            if (string.IsNullOrEmpty(clientId))
            {
                _logger.LogWarning("RequestCorrelationId: {CorrelationId} - Missing clientId.", requestCorrelationId);
                return Unauthorized();
            }

            var bagRegistration = await _bagRegistrationRepo.GetBagByNumberForClientAsync(bagNumber, clientId);
            if (bagRegistration == null)
            {
                return NotFound(BagRegSummaryResponse.CreateNotFoundSummary(bagNumber, clientId, requestCorrelationId));
            }

            bagRegistration.OperationHistory.Add(new OperationHistoryEntry
            {
                Action = "Read",
                Timestamp = DateTime.UtcNow,
                BagLifeCycleId = bagRegistration.CorrelationId,
                RequestCorrelationId = requestCorrelationId,
                PerformedBy = clientId
            });

            await _bagRegistrationRepo.UpdateBagRegistration(bagRegistration);

            var bagRegRes = _mapper.Map<BagRegistrationRequestForCreation>(bagRegistration);

            return Ok(bagRegRes);
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(BagRegSummaryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BagRegSummaryResponse>> CreateBagRegistration(BagRegistrationRequestForCreation bagRequest)
        {
            var BagLifecycleId = Guid.NewGuid().ToString();
            var requestCorrelationId = GetCorrelationId();
            var clientId = ClaimsHelper.GetClientId(User);

            if (bagRequest == null || !ModelState.IsValid)
            {
                _logger.LogError("CorrelationId: {CorrelationId} - Invalid model or bad request for {RequestPath}.", requestCorrelationId, Request.Path);
                return BadRequest("Invalid request data");
            }

            await _serviceBusSender.SendMessageAsync(JsonSerializer.Serialize(bagRequest), "BagRegistration");

            var rawPayload = await Request.ReadRawBodyAsStringAsync();
            await _blobArchiveService.ArchivePayloadAsync($"bag_{DateTime.UtcNow:yyyyMMddHHmmss}", rawPayload);

            var newBagRegistrationRequest = _mapper.Map<BagRegistrationRequest>(bagRequest);
            newBagRegistrationRequest.ClientId = clientId;
            newBagRegistrationRequest.BagLifecycleId = BagLifecycleId;
            newBagRegistrationRequest.CorrelationId = requestCorrelationId;

            newBagRegistrationRequest.OperationHistory.Add(new OperationHistoryEntry
            {
                Action = "Create",
                Timestamp = DateTime.UtcNow,
                BagLifeCycleId = newBagRegistrationRequest.BagLifecycleId,
                RequestCorrelationId = requestCorrelationId,
                PerformedBy = clientId
            });

            await _bagRegistrationRepo.CreateBagRegistration(newBagRegistrationRequest);

            var bagSummary = new BagRegSummaryResponse
            {
                CustomerNumber = newBagRegistrationRequest.BagRegistration.CustomerNumber,
                CustomerName = newBagRegistrationRequest.BagRegistration.CustomerName,
                ActionFlag = newBagRegistrationRequest.BagRegistration.ActionFlag,
                BagNumber = newBagRegistrationRequest.BagRegistration.BagNumber,
                RegistrationStatus = newBagRegistrationRequest.RegistrationStatus,
                Description = "Bag Created Successfully",
                BagLifecycleId = newBagRegistrationRequest.BagLifecycleId,
                RequestCorrelationId = requestCorrelationId
            };

            _logger.LogInformation("BagLifecycleId: {BagLifecycleId} , CorrelationId: {CorrelationId} - Bag registration created successfully.", BagLifecycleId, requestCorrelationId);

            return Ok(bagSummary);
        }

        [HttpPut("{bagNumber}")]
        [ProducesResponseType(typeof(BagRegSummaryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BagRegSummaryResponse>> UpdateBagRegistration(string bagNumber, [FromBody] BagRegistrationRequestForCreation updatedPayload)
        {
            var requestCorrelationId = GetCorrelationId();
            var clientId = ClaimsHelper.GetClientId(User);

            if (updatedPayload?.BagRegistration == null)
            {
                _logger.LogWarning("CorrelationId: {CorrelationId} - Update request payload is missing or invalid.", requestCorrelationId);
                return BadRequest("The request body is missing or malformed.");
            }

            if (!string.Equals(bagNumber, updatedPayload.BagRegistration.BagNumber, StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning("CorrelationId: {CorrelationId} - Mismatch between BagNumber in URL ({UrlBag}) and payload ({PayloadBag}).",
                    requestCorrelationId, bagNumber, updatedPayload.BagRegistration.BagNumber);
                return BadRequest("BagNumber in the URL and in the payload do not match.");
            }

            var existing = await _bagRegistrationRepo.GetBagByNumberForClientAsync(bagNumber, clientId);
            if (existing == null)
            {
                _logger.LogWarning("CorrelationId: {CorrelationId} - Bag {BagNumber} not found for update.", requestCorrelationId, bagNumber);
                return NotFound(BagRegSummaryResponse.CreateNotFoundSummary(bagNumber, clientId, requestCorrelationId));

            }

            await _serviceBusSender.SendMessageAsync(JsonSerializer.Serialize(updatedPayload), "BagRegistration");
            var rawPayload = await Request.ReadRawBodyAsStringAsync();
            await _blobArchiveService.ArchivePayloadAsync($"bag_{DateTime.UtcNow:yyyyMMddHHmmss}", rawPayload);

            var updatedBag = _mapper.Map<Models.BagRegistration>(updatedPayload.BagRegistration);
            existing.BagRegistration = updatedBag;
            existing.DownstreamSystemId = existing.DownstreamSystemId;
            existing.RegistrationType = updatedPayload.RegistrationType;
            existing.CustomerCountry = updatedPayload.CustomerCountry;
            existing.RegistrationStatus = "Updated";

            existing.OperationHistory.Add(new OperationHistoryEntry
            {
                Action = "Update",
                Timestamp = DateTime.UtcNow,
                BagLifeCycleId = existing.CorrelationId,
                PerformedBy = clientId
            });

            await _bagRegistrationRepo.UpdateBagRegistration(existing);

            var response = _mapper.Map<BagRegSummaryResponse>(existing);
            response.Description = "Bag registration updated successfully.";
            response.RequestCorrelationId = requestCorrelationId;
            return Ok(response);
        }

        [HttpDelete("{bagNumber}")]
        [ProducesResponseType(typeof(BagRegSummaryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BagRegSummaryResponse>> DeleteBagRegistration(string bagNumber)
        {
            var requestCorrelationId = GetCorrelationId();
            var clientId = ClaimsHelper.GetClientId(User);

            var existing = await _bagRegistrationRepo.GetBagByNumberForClientAsync(bagNumber, clientId);
            if (existing == null)
                return NotFound();

            existing.RegistrationStatus = "Deleted";

            existing.OperationHistory.Add(new OperationHistoryEntry
            {
                Action = "Delete",
                Timestamp = DateTime.UtcNow,
                BagLifeCycleId = existing.CorrelationId,
                RequestCorrelationId = requestCorrelationId,
                PerformedBy = clientId
            });

            await _bagRegistrationRepo.UpdateBagRegistration(existing);

            var response = _mapper.Map<BagRegSummaryResponse>(existing);
            response.Description = "Bag deleted successfully.";
            response.RequestCorrelationId = requestCorrelationId;
            response.BagLifecycleId = existing.BagLifecycleId;
            return Ok(response);
        }
    }
}
