using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Nokas.CashBagManagement.WebAPI.Models
{
    [XmlRoot("BagRegistrationRequest")]
    public class BagRegistrationRequestForCreation
    {
        [XmlElement("BagRegistration")]
        public BagRegistration BagRegistration { get; set; }

        [XmlElement("CacheDbRegistrationId")]
        public string CacheDbRegistrationId { get; set; }

        [XmlElement("RegistrationType")]
        public string RegistrationType { get; set; }

        [XmlElement("CustomerCountry")]
        public string CustomerCountry { get; set; }
    }
}
