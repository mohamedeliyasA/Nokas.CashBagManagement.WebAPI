using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Nokas.CashBagManagement.WebAPI.Models
{
    public class BagRegistration
    {
        [XmlElement("CustomerNumber")]
        public string CustomerNumber { get; set; }

        [XmlElement("CustomerName")]
        public string CustomerName { get; set; }

        [XmlElement("CustomerCountry")]
        public string CustomerCountry { get; set; }

        [XmlElement("RegistrationType")]
        public string RegistrationType { get; set; }

        [XmlElement("ServiceNowOrderNumber")]
        public string ServiceNowOrderNumber { get; set; }

        [XmlElement("DownstreamOrderNumber")]
        public string DownstreamOrderNumber { get; set; }

        [XmlElement("MessageId")]
        public string MessageId { get; set; }

        [XmlElement("SenderSystem")]
        public string SenderSystem { get; set; }

        [XmlElement("SenderCorrId")]
        public string SenderCorrId { get; set; }

        [XmlElement("BagNumber")]
        public string BagNumber { get; set; }

        [XmlElement("ActionFlag")]
        public string ActionFlag { get; set; }

        [XmlElement("RegistrationDate")]
        public string RegistrationDate { get; set; }

        [XmlElement("ReferenceStatement")]
        public string ReferenceStatement { get; set; }

        [XmlElement("TurnOverDate")]
        public string TurnOverDate { get; set; }

        [XmlElement("RegisteredBy")]
        public string RegisteredBy { get; set; }

        [XmlElement("EasySafeAccount")]
        public string EasySafeAccount { get; set; }

        [XmlElement("RegisteredCash")]
        public string RegisteredCash { get; set; }

        [XmlElement("RegisteredCoins")]
        public string RegisteredCoins { get; set; }

        [XmlElement("RegisteredChecks")]
        public string RegisteredChecks { get; set; }

        [XmlElement("TotalOrderAmountLC")]
        public string TotalOrderAmountLC { get; set; }

        [XmlElement("TotalOrderAmountFC")]
        public string TotalOrderAmountFC { get; set; }

        [XmlElement("BagItemsLC")]
        public BagItemsLC BagItemsLC { get; set; }

        [XmlArray("BagItemsFC")]
        [XmlArrayItem("BagItemFC")]
        public List<BagItemFC> BagItemsFC { get; set; }
    }

    public class BagItemsLC
    {
        [XmlElement("Seddel1000")]
        public string Seddel1000 { get; set; }

        [XmlElement("Seddel500")]
        public string Seddel500 { get; set; }

        [XmlElement("Seddel200")]
        public string Seddel200 { get; set; }

        [XmlElement("Seddel100")]
        public string Seddel100 { get; set; }

        [XmlElement("Seddel50")]
        public string Seddel50 { get; set; }
    }

    public class BagItemFC
    {
        [XmlElement("ItemId")]
        public string ItemId { get; set; }

        [XmlElement("ItemCurrencyCode")]
        public string ItemCurrencyCode { get; set; }

        [XmlElement("Quantity")]
        public string Quantity { get; set; }

        [XmlElement("BaseCurrencyCode")]
        public string BaseCurrencyCode { get; set; }

        [XmlElement("ItemCurrencyAmountFC")]
        public string ItemCurrencyAmountFC { get; set; }

        [XmlElement("ItemCurrencyAmountLC")]
        public string ItemCurrencyAmountLC { get; set; }

        [XmlElement("ExchangeRate")]
        public string ExchangeRate { get; set; }

        [XmlElement("ItemUnitPrice")]
        public string ItemUnitPrice { get; set; }
    }
}