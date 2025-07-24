using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Nokas.CashBagManagement.WebAPI.Models
{
    [XmlRoot("BagRegistrationRequest")]
    public class BagRegistrationRequestForCreation
    {
        [Required]
        [XmlElement("BagRegistration")]
        public BagRegistration BagRegistration { get; set; }

        [Required]
        [XmlElement("RegistrationType")]
        public string RegistrationType { get; set; }

        [Required]
        [XmlElement("CustomerCountry")]
        public string CustomerCountry { get; set; }
    }
}
