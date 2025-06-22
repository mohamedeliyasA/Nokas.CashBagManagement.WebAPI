namespace Nokas.CashBagManagement.WebAPI.Models
{
    public class OperationHistoryEntry
    {
        public string Action { get; set; } // "Create", "Update", "Delete"
        public DateTime Timestamp { get; set; }
        public string CorrelationId { get; set; }
        public string PerformedBy { get; set; } // e.g., client app ID from JWT

        public string RequestCorrelationId { get; set; }
    }
}
