
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nokas.CashBagManagement.WebAPI.Models;

namespace Nokas.CashBagManagement.WebAPI.Controllers
{
    [Route("api/cashbag/register")]
    [ApiController]
    public class CashBagRegistration : ControllerBase
    {
        [HttpGet]

        public ActionResult<CashBagRegistrationDto> GetCashBag()
        {
            var cashBag = CashBagRegistrationDataStore.Current.CashBagRegistrationDto;
            return Ok(cashBag);

        }
    }
}
