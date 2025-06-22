
using System.Xml.Serialization;

namespace Nokas.CashBagManagement.WebAPI.Models
{
    public class BagRegistration
    {
        [XmlElement("ActionFlag")]
        public string ActionFlag { get; set; }

        [XmlElement("CustomerNumber")]
        public string CustomerNumber { get; set; }

        [XmlElement("CustomerName")]
        public string CustomerName { get; set; }

        [XmlElement("BankInfo")]
        public BankInfo BankInfo { get; set; }

        [XmlElement("CustomerAddress")]
        public string CustomerAddress { get; set; }

        [XmlElement("RegisteredName")]
        public string RegisteredName { get; set; }

        [XmlElement("BagNumber")]
        public string BagNumber { get; set; }

        [XmlElement("TurnoverDate")]
        public DateTime TurnoverDate { get; set; }

        [XmlElement("ReferenceStatement")]
        public string ReferenceStatement { get; set; }

        [XmlArray("ExchangeRates")]
        [XmlArrayItem("ExchangeRate")]
        public List<ExchangeRate> ExchangeRates { get; set; }

        [XmlElement("RegistrationDate")]
        public DateTime RegistrationDate { get; set; }

        [XmlElement("RegisteredBy")]
        public string RegisteredBy { get; set; }

        [XmlElement("RegisteredCoins")]
        public string RegisteredCoins { get; set; }

        [XmlElement("RegisteredCash")]
        public string RegisteredCash { get; set; }

        [XmlElement("RegisteredChecks")]
        public string RegisteredChecks { get; set; }

        [XmlElement("RegisteredForeignCurrency")]
        public string RegisteredForeignCurrency { get; set; }

        [XmlElement("CountedAmount")]
        public string CountedAmount { get; set; }

        [XmlElement("CountedDate")]
        public DateTime CountedDate { get; set; }

        [XmlElement("TotalAmount")]
        public string TotalAmount { get; set; }

        [XmlElement("LocationId")]
        public string LocationId { get; set; }

        [XmlElement("Location")]
        public string Location { get; set; }

        [XmlElement("ShopNumber")]
        public string ShopNumber { get; set; }

        [XmlElement("EasySafeAccount")]
        public string EasySafeAccount { get; set; }

        [XmlElement("RegistrationApproval")]
        public string RegistrationApproval { get; set; }

        [XmlElement("IsBankAccount")]
        public string IsBankAccount { get; set; }

        [XmlElement("Notes")]
        public Notes Notes { get; set; }

        [XmlElement("RegistrationStatus")]
        public string RegistrationStatus { get; set; }

        [XmlElement("Contracts")]
        public Contracts Contracts { get; set; }

        [XmlElement("PickedUpDate")]
        public DateTime PickedUpDate { get; set; }

        [XmlElement("NightSafeId")]
        public string NightSafeId { get; set; }

        [XmlElement("RegistrationSubType")]
        public string RegistrationSubType { get; set; }

        [XmlArray("ForeignCurrencies")]
        [XmlArrayItem("ForeignCurrency")]
        public List<ForeignCurrency> ForeignCurrencies { get; set; }

        [XmlElement("CustomerCountry")]
        public string CustomerCountry { get; set; }

        [XmlElement("ConfirmFlag")]
        public string ConfirmFlag { get; set; }

        [XmlElement("RegisteredUserId")]
        public string RegisteredUserId { get; set; }

        [XmlElement("ConfirmDateTime")]
        public DateTime ConfirmDateTime { get; set; }

        [XmlElement("DropNSName")]
        public string DropNSName { get; set; }

        [XmlElement("DropNSDateTime")]
        public DateTime DropNSDateTime { get; set; }

        [XmlArray("Vouchers")]
        [XmlArrayItem("VoucherDetails")]
        public List<VoucherDetail> Vouchers { get; set; }
    }

    public class BankInfo
    {
        public string ActingBranchName { get; set; }
        public string ActingBranchId { get; set; }
        public string MainBankName { get; set; }
        public string MainBankId { get; set; }
    }

    public class ExchangeRate
    {
        public string ForeignCurrencyCode { get; set; }
        public string ForeignCurrencyAmt { get; set; }

        [XmlElement("ExchangeRate")]
        public string ExchangeRateValue { get; set; }

        public string AmountInLocalCurrency { get; set; }
    }

    public class Notes
    {
        public string Seddel1000 { get; set; }
        public string Seddel500 { get; set; }
        public string Seddel200 { get; set; }
        public string Seddel100 { get; set; }
        public string Seddel50 { get; set; }
    }

    public class Contracts
    {
        public string IsPDADeclared { get; set; }
        public string DeviationType { get; set; }
        public string DeclarationType { get; set; }
        public string ContainsValuta { get; set; }
        public string ControlledAmount { get; set; }
    }

    public class ForeignCurrency
    {
        public string CurrencyISOCode { get; set; }
        public string BuyNotes { get; set; }
        public string NotesAmountNok { get; set; }
        public string NotesAmountCurrency { get; set; }
        public string BuyCheques { get; set; }
        public string ChequesAmountNOK { get; set; }
        public string ChequesAmountCurrency { get; set; }
        public string CurrencyAmount { get; set; }

        [XmlElement("RegDate")]
        public DateTime RegDate { get; set; }

        public string ItemId { get; set; }

        [XmlElement("ExchangeRate")]
        public string ExchangeRate { get; set; }
    }

    public class VoucherDetail
    {
        public string VoucherNumber { get; set; }
        public string VoucherAmount { get; set; }
        public string BankBranchNumber { get; set; }
        public string BankBranchName { get; set; }
    }
}
