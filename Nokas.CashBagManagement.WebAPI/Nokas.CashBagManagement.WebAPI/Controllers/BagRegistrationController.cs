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

        /// <summary>
        /// Retrieves a bag registration by its bag number.
        /// </summary>
        /// <param name="bagNumber">The bag number to retrieve.</param>
        /// <returns>A summary of the bag registration.</returns>
        [HttpGet("{bagNumber}", Name = "GetBagRegistration")]
        [ProducesResponseType(typeof(BagRegSummaryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BagRegSummaryResponse>> GetBagRegistrationByNumber(string bagNumber)
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
                return NotFound($"No bag found with the BagNumber: {bagNumber} for this client with Id: {clientId}.");
            }

            bagRegistration.OperationHistory.Add(new OperationHistoryEntry
            {
                Action = "Read",
                Timestamp = DateTime.UtcNow,
                CorrelationId = bagRegistration.CorrelationId,
                RequestCorrelationId = requestCorrelationId,
                PerformedBy = clientId
            });

            await _bagRegistrationRepo.UpdateBagRegistration(bagRegistration);

            var bagSummary = _mapper.Map<BagRegSummaryResponse>(bagRegistration);
            bagSummary.Description = "Bag Retrieved Successfully";
            bagSummary.RequestCorrelationId = requestCorrelationId;

            return Ok(bagSummary);
        }

        /// <summary>
        /// Creates a new bag registration.
        /// </summary>
        /// <param name="bagRequest">The bag registration request payload.</param>
        /// <returns>Summary of the created bag registration.</returns>
        [HttpPost("register")]
        [ProducesResponseType(typeof(BagRegSummaryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BagRegSummaryResponse>> CreateBagRegistration(BagRegistrationRequestForCreation bagRequest)
        {
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
            newBagRegistrationRequest.CorrelationId = requestCorrelationId;

            newBagRegistrationRequest.OperationHistory.Add(new OperationHistoryEntry
            {
                Action = "Create",
                Timestamp = DateTime.UtcNow,
                CorrelationId = newBagRegistrationRequest.CorrelationId,
                RequestCorrelationId = requestCorrelationId,
                PerformedBy = clientId
            });

            await _bagRegistrationRepo.CreateBagRegistration(newBagRegistrationRequest);

            var bagSummary = _mapper.Map<BagRegSummaryResponse>(newBagRegistrationRequest);
            bagSummary.Description = "Bag Created Successfully";
            bagSummary.RequestCorrelationId = requestCorrelationId;

            _logger.LogInformation("CorrelationId: {CorrelationId} - Bag registration created successfully.", requestCorrelationId);

            return Ok(bagSummary);
        }

        /// <summary>
        /// Updates an existing bag registration by its number.
        /// </summary>
        /// <param name="bagNumber">The bag number to update.</param>
        /// <param name="updatedPayload">The updated registration payload.</param>
        /// <returns>Summary of the updated bag registration.</returns>
        [HttpPut("{bagNumber}")]
        [ProducesResponseType(typeof(BagRegSummaryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BagRegSummaryResponse>> UpdateBagRegistration(string bagNumber, [FromBody] BagRegistrationRequestForCreation updatedPayload)
        {
            var requestCorrelationId = GetCorrelationId();
            var clientId = ClaimsHelper.GetClientId(User);

            var existing = await _bagRegistrationRepo.GetBagByNumberForClientAsync(bagNumber, clientId);
            if (existing == null)
            {
                _logger.LogWarning("CorrelationId: {RequestCorrelationId} - Bag {BagNumber} not found for update.", requestCorrelationId, bagNumber);
                return NotFound("Bag not found.");
            }

            await _serviceBusSender.SendMessageAsync(JsonSerializer.Serialize(updatedPayload), "BagRegistration");

            var rawPayload = await Request.ReadRawBodyAsStringAsync();
            await _blobArchiveService.ArchivePayloadAsync($"bag_{DateTime.UtcNow:yyyyMMddHHmmss}", rawPayload);

            var updatedBag = _mapper.Map<Models.BagRegistration>(updatedPayload.BagRegistration);
            existing.BagRegistration = updatedBag;
            existing.CacheDbRegistrationId = updatedPayload.CacheDbRegistrationId;
            existing.RegistrationType = updatedPayload.RegistrationType;
            existing.CustomerCountry = updatedPayload.CustomerCountry;
            existing.Status = "Updated";

            existing.OperationHistory.Add(new OperationHistoryEntry
            {
                Action = "Update",
                Timestamp = DateTime.UtcNow,
                CorrelationId = existing.CorrelationId,
                PerformedBy = clientId
            });

            await _bagRegistrationRepo.UpdateBagRegistration(existing);

            var response = _mapper.Map<BagRegSummaryResponse>(existing);
            response.Description = "Bag registration updated successfully.";
            response.RequestCorrelationId = requestCorrelationId;
            return Ok(response);
        }

        /// <summary>
        /// Deletes a bag registration by its bag number.
        /// </summary>
        /// <param name="bagNumber">The bag number to delete.</param>
        /// <returns>Summary of the deleted bag registration.</returns>
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

            existing.Status = "Deleted";

            existing.OperationHistory.Add(new OperationHistoryEntry
            {
                Action = "Delete",
                Timestamp = DateTime.UtcNow,
                CorrelationId = existing.CorrelationId,
                RequestCorrelationId = requestCorrelationId,
                PerformedBy = clientId
            });

            await _bagRegistrationRepo.UpdateBagRegistration(existing);

            var response = _mapper.Map<BagRegSummaryResponse>(existing);
            response.Description = "Bag deleted successfully.";
            response.RequestCorrelationId = requestCorrelationId;
            response.CorrelationId = existing.CorrelationId;
            return Ok(response);
        }
    }
}
