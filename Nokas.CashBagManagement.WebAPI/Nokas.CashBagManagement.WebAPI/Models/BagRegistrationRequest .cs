using System.Collections.Generic;
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

        /// <summary>
        /// Used internally to track registration lifecycle. Not required from consumer.
        /// </summary>
        [XmlElement("RegistrationStatus")]
        public string RegistrationStatus { get; set; } = "Registered"; // Default to Registered

        [JsonProperty("clientId")]
        public string ClientId { get; set; }

        public string CorrelationId { get; set; }

        public List<OperationHistoryEntry> OperationHistory { get; set; } = new();
    }
}
