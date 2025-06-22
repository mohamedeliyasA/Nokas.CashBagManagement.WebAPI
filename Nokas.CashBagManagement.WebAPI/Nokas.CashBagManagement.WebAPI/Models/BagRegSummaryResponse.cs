namespace Nokas.CashBagManagement.WebAPI.Models
{
    public class BagRegSummaryResponse
    {
        public string CustomerNumber { get; set; }
        public string CustomerName { get; set; }
        public string BagNumber { get; set; }
        public string ActionFlag { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string CacheDbRegistrationId { get; set; }

        public string CorrelationId { get; set; }
        public string RequestCorrelationId { get; set; }
    }

}
