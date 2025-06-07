using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
namespace Nokas.CashBagManagement.WebAPI.Models
{
    [XmlRoot("CashBagRegistrationForCreationDto")]
    public class CashBagRegistrationForCreationDto
    {
        [Required]
        [XmlElement("registrations")]
        public CreateRegistration Registrations { get; set; } = new();
        [Required]
        [XmlElement("cacheDbRegistrationId")]
        public int CacheDbRegistrationId { get; set; }
        [Required]
        [XmlElement("customerCountry")]
        public string CustomerCountry { get; set; } = string.Empty;
    }
    public class CreateRegistration
    {
        // Identification and meta
        [XmlElement("actionFlag")]
        public string ActionFlag { get; set; } = string.Empty;
        [XmlElement("customerNumber")]
        public string CustomerNumber { get; set; } = string.Empty;
        [XmlElement("customerAddress")]
        public string CustomerAddress { get; set; } = string.Empty;
        [XmlElement("bagNumber")]
        public string BagNumber { get; set; } = string.Empty;
        [XmlElement("turnoverDate")]
        public DateTime TurnoverDate { get; set; }
        [XmlElement("registrationDate")]
        public DateTime RegistrationDate { get; set; }
        [XmlElement("registeredBy")]
        public string RegisteredBy { get; set; } = string.Empty;
        [XmlElement("registeredUserId")]
        public string RegisteredUserId { get; set; } = string.Empty;
        [XmlElement("registrationApproval")]
        public int RegistrationApproval { get; set; }
        [XmlElement("registrationSubType")]
        public string RegistrationSubType { get; set; } = string.Empty;
        [XmlElement("referenceStatement")]
        public string ReferenceStatement { get; set; } = string.Empty;
        [XmlElement("confirmFlag")]
        public string ConfirmFlag { get; set; } = string.Empty;
        // Financials
        [XmlElement("registeredCoins")]
        public decimal RegisteredCoins { get; set; }
        [XmlElement("registeredCash")]
        public decimal RegisteredCash { get; set; }
        [XmlElement("registeredChecks")]
        public decimal RegisteredChecks { get; set; }
        [XmlElement("registeredForeignCurrency")]
        public decimal RegisteredForeignCurrency { get; set; }
        [XmlElement("totalAmount")]
        public decimal TotalAmount { get; set; }
        // Location and logistics
        [XmlElement("locationId")]
        public string LocationId { get; set; } = string.Empty;
        [XmlElement("shopNumber")]
        public string ShopNumber { get; set; } = string.Empty;
        [XmlElement("easySafeAccount")]
        public string EasySafeAccount { get; set; } = string.Empty;
        [XmlElement("nightSafeId")]
        public string NightSafeId { get; set; } = string.Empty;
        [XmlElement("foreignCurrencies")]
        public string ForeignCurrencies { get; set; } = string.Empty;
        // Complex types
        [XmlElement("exchangeRates")]
        public CreateExchangeRates ExchangeRates { get; set; } = new();
        [XmlElement("notes")]
        public CreateNotes Notes { get; set; } = new();
        [XmlElement("contracts")]
        public CreateContracts Contracts { get; set; } = new();
        [XmlElement("vouchers")]
        public List<CreateVouchers> Vouchers { get; set; } = new();
    }
    public class CreateExchangeRates
    {
        [XmlElement("exchangeRate")]
        public string ExchangeRate { get; set; } = string.Empty;
    }
    public class CreateNotes
    {
        [XmlElement("seddel1000")]
        public string Seddel1000 { get; set; } = string.Empty;
        [XmlElement("seddel500")]
        public string Seddel500 { get; set; } = string.Empty;
        [XmlElement("seddel200")]
        public string Seddel200 { get; set; } = string.Empty;
        [XmlElement("seddel100")]
        public string Seddel100 { get; set; } = string.Empty;
        [XmlElement("seddel50")]
        public string Seddel50 { get; set; } = string.Empty;
    }
    public class CreateContracts
    {
        [XmlElement("containsValuta")]
        public bool ContainsValuta { get; set; }
    }
    public class CreateVouchers
    {
        [XmlElement("voucherDetails")]
        public string VoucherDetails { get; set; } = string.Empty;
    }
}