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

        public CashBagRegistrationController(ILogger<CashBagRegistrationController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // Helper method to get the correlation ID
        private string GetCorrelationId() =>
            HttpContext?.Items["CorrelationId"]?.ToString() ?? "N/A"; // Fallback to "N/A" if no correlation ID is set

        [HttpGet(Name = "GetCashBagRegistrationData")]
        public ActionResult<CashBagRegistrationDto> GetCashBag()
        {
            

            var correlationId = GetCorrelationId();
            try
            {
                var cashBag = CashBagRegistrationDataStore.Current.CashBagRegistrationDto;
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
        public ActionResult<CashBagRegistrationDto> CreateBagRegistration(CashBagRegistrationForCreationDto bagRegistration)
        {
            var correlationId = GetCorrelationId();
            if (bagRegistration == null || !ModelState.IsValid)
            {
                // Log the BadRequest (400)
                _logger.LogError("CorrelationId: {CorrelationId} - Invalid model or bad request for {RequestPath}.", correlationId, Request.Path);
                return BadRequest("Invalid request data");
            }
            try
            {
                var cashBagRegistrationDto = new CashBagRegistrationDto
                {
                    CacheDbRegistrationId = bagRegistration.CacheDbRegistrationId,
                    CustomerCountry = bagRegistration.CustomerCountry,
                    Registrations = new Registration
                    {
                        ActionFlag = bagRegistration.Registrations.ActionFlag,
                        CustomerNumber = bagRegistration.Registrations.CustomerNumber,
                        CustomerAddress = bagRegistration.Registrations.CustomerAddress,
                        BagNumber = bagRegistration.Registrations.BagNumber,
                        TurnoverDate = bagRegistration.Registrations.TurnoverDate,
                        ReferenceStatement = bagRegistration.Registrations.ReferenceStatement,
                        ExchangeRates = new ExchangeRates
                        {
                            ExchangeRate = bagRegistration.Registrations.ExchangeRates.ExchangeRate
                        },
                        RegistrationDate = bagRegistration.Registrations.RegistrationDate,
                        RegisteredBy = bagRegistration.Registrations.RegisteredBy,
                        RegisteredCoins = bagRegistration.Registrations.RegisteredCoins,
                        RegisteredCash = bagRegistration.Registrations.RegisteredCash,
                        RegisteredChecks = bagRegistration.Registrations.RegisteredChecks,
                        RegisteredForeignCurrency = bagRegistration.Registrations.RegisteredForeignCurrency,
                        TotalAmount = bagRegistration.Registrations.TotalAmount,
                        LocationId = bagRegistration.Registrations.LocationId,
                        ShopNumber = bagRegistration.Registrations.ShopNumber,
                        EasySafeAccount = bagRegistration.Registrations.EasySafeAccount,
                        RegistrationApproval = bagRegistration.Registrations.RegistrationApproval,
                        Notes = new Notes
                        {
                            Seddel1000 = bagRegistration.Registrations.Notes.Seddel1000,
                            Seddel500 = bagRegistration.Registrations.Notes.Seddel500,
                            Seddel200 = bagRegistration.Registrations.Notes.Seddel200,
                            Seddel100 = bagRegistration.Registrations.Notes.Seddel100,
                            Seddel50 = bagRegistration.Registrations.Notes.Seddel50
                        },
                        Contracts = new Contracts
                        {
                            ContainsValuta = bagRegistration.Registrations.Contracts.ContainsValuta
                        },
                        NightSafeId = bagRegistration.Registrations.NightSafeId,
                        RegistrationSubType = bagRegistration.Registrations.RegistrationSubType,
                        ForeignCurrencies = bagRegistration.Registrations.ForeignCurrencies,
                        ConfirmFlag = bagRegistration.Registrations.ConfirmFlag,
                        RegisteredUserId = bagRegistration.Registrations.RegisteredUserId,
                        Vouchers = bagRegistration.Registrations.Vouchers
                                    .Select(v => new Vouchers
                                    {
                                        VoucherDetails = v.VoucherDetails
                                    })
                                    .ToList()
                    }
                };

                // Assign to the static data store so GET will return the new data
                CashBagRegistrationDataStore.Current.CashBagRegistrationDto = cashBagRegistrationDto;

                _logger.LogInformation("CorrelationId: {CorrelationId} - Successfully created bag registration.", correlationId);
                return CreatedAtRoute("GetCashBagRegistrationData", cashBagRegistrationDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CorrelationId: {CorrelationId} - An error occurred while creating the bag registration.", correlationId);
                return StatusCode(StatusCodes.Status500InternalServerError, "An internal server error occurred.");
            }
        }
    }
}
