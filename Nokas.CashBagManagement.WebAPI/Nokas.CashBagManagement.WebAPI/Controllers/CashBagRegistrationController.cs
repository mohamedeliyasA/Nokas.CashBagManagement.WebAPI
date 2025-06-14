using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nokas.CashBagManagement.WebAPI.Models;

namespace Nokas.CashBagManagement.WebAPI.Controllers
{
    [Authorize]
    [Route("api/cashbag/register")]
    [ApiController]
    public class CashBagRegistrationController : ControllerBase
    {
        private readonly ILogger<CashBagRegistrationController> _logger;
        private readonly BagRegistrationDataStore _cashBagRegistrationDataStore;
        private readonly IMapper _mapper;

        public CashBagRegistrationController(ILogger<CashBagRegistrationController> logger, BagRegistrationDataStore cashBagRegistrationDataStore, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cashBagRegistrationDataStore = cashBagRegistrationDataStore ?? throw new ArgumentNullException(nameof(cashBagRegistrationDataStore));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // Helper method to get the correlation ID
        private string GetCorrelationId() =>
            HttpContext?.Items["CorrelationId"]?.ToString() ?? "N/A"; // Fallback to "N/A" if no correlation ID is set

        [HttpGet(Name = "GetCashBagRegistrationData")]
        public ActionResult<BagRegistrationRequest> GetCashBag()
        {
            

            var correlationId = GetCorrelationId();
            try
            {
                var cashBag = _cashBagRegistrationDataStore.BagRegistrationRequest;
                if (cashBag == null)
                {
                    _logger.LogWarning("CorrelationId: {CorrelationId} - Cash bag registration data not found.", correlationId);
                    return NotFound("No cash bag registration data found.");
                }
                return Ok(cashBag);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CorrelationId: {CorrelationId} - An error occurred while retrieving the cash bag registration data.", correlationId);
                return StatusCode(StatusCodes.Status500InternalServerError, "An internal server error occurred.");
            }
        }

        [HttpPost]
        public ActionResult<BagRegistrationRequest> CreateBagRegistration(BagRegistrationRequestForCreation bagRequest)
        {
            var correlationId = GetCorrelationId();

            if (bagRequest == null || !ModelState.IsValid)
            {
                _logger.LogError("CorrelationId: {CorrelationId} - Invalid model or bad request for {RequestPath}.", correlationId, Request.Path);
                return BadRequest("Invalid request data");
            }

            if (bagRequest.BagRegistration == null)
            {
                _logger.LogWarning("CorrelationId: {CorrelationId} - BagRegistration is null.", correlationId);
                return BadRequest("BagRegistration must be provided.");
            }

            try
            {
                var mappedBagRegistration = _mapper.Map<BagRegistration>(bagRequest.BagRegistration);

                var newBagRegistrationRequest = new BagRegistrationRequest
                {
                    BagRegistration = mappedBagRegistration,
                    CacheDbRegistrationId = bagRequest.CacheDbRegistrationId,
                    RegistrationType = bagRequest.RegistrationType,
                    CustomerCountry = bagRequest.CustomerCountry
                };

                // Optionally, store the new registration in the data store
                _cashBagRegistrationDataStore.BagRegistrationRequest = newBagRegistrationRequest;

                _logger.LogInformation("CorrelationId: {CorrelationId} - Successfully created bag registration.", correlationId);
                return CreatedAtRoute("GetCashBagRegistrationData", newBagRegistrationRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CorrelationId: {CorrelationId} - An error occurred while creating the bag registration.", correlationId);
                return StatusCode(StatusCodes.Status500InternalServerError, "An internal server error occurred.");
            }
        }
    }
}
