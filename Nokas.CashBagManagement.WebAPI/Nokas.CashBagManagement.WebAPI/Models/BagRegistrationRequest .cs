
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Nokas.CashBagManagement.WebAPI.Models
{
    [XmlRoot("BagRegistrationRequest")]
    public class BagRegistrationRequest
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [XmlElement("BagRegistration")]
        public BagRegistration BagRegistration { get; set; }

        [XmlElement("CacheDbRegistrationId")]
        public string CacheDbRegistrationId { get; set; }

        [XmlElement("RegistrationType")]
        public string RegistrationType { get; set; }

        [XmlElement("CustomerCountry")]
        public string CustomerCountry { get; set; }

        [XmlElement("Status")]
        public string Status { get; set; } = "Registered"; // Optional default
        [JsonProperty("clientId")] 
        public string ClientId { get; set; }
        public string CorrelationId { get; set; }

        public List<OperationHistoryEntry> OperationHistory { get; set; } = new();
    }
}
