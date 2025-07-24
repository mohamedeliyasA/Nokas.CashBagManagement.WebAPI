namespace Nokas.CashBagManagement.WebAPI.Models
{
    public class BagRegSummaryResponse
    {
        public string CustomerNumber { get; set; }
        public string CustomerName { get; set; }
        public string BagNumber { get; set; }
        public string ActionFlag { get; set; }
        public string Description { get; set; }
        public string RegistrationStatus { get; set; }
        public string DownstreamSystemId { get; set; }

        public string BagLifecycleId { get; set; }
        public string RequestCorrelationId { get; set; }

        public static BagRegSummaryResponse CreateNotFoundSummary(string bagNumber, string clientId, string requestCorrelationId)
        {
            return new BagRegSummaryResponse
            {
                CustomerNumber = "NA",
                CustomerName = "NA",
                ActionFlag = "NA",
                BagNumber = "NA",
                RegistrationStatus = "NA",
                Description = $"No bag found with the BagNumber: {bagNumber} for this client with Id: {clientId}.",
                BagLifecycleId = "NA",
                RequestCorrelationId = requestCorrelationId
            };
        }
    }

}
