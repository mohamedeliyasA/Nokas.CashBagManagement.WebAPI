namespace Nokas.CashBagManagement.WebAPI.Models
{
    public class BagRegistrationDetailsResponse
    {
        public BagRegistration BagRegistration { get; set; }

        public string CacheDbRegistrationId { get; set; }

        public string RegistrationType { get; set; }

        public string CustomerCountry { get; set; }

        public string Status { get; set; }
    }
}
