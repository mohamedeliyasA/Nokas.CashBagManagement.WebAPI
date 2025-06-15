namespace Nokas.CashBagManagement.WebAPI.Models
{
    public class BagRegistrationResponse
    {
        public string BagNumber { get; set; }
        public string Status { get; set; } = "Success";
        public string Message { get; set; }
        public string CorrelationId { get; set; }
    }
}
