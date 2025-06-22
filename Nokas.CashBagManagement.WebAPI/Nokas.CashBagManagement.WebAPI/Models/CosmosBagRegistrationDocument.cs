using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Nokas.CashBagManagement.WebAPI.CosmosModels
{
    public class CosmosBagRegistrationDocument
    {
        [JsonProperty("id")]
        public string Id => BagRegistration?.BagNumber;

        [JsonProperty("clientId")]
        public string ClientId { get; set; }

        [JsonProperty("bagRegistration")]
        public BagRegistration BagRegistration { get; set; }

        [JsonProperty("cacheDbRegistrationId")]
        public string CacheDbRegistrationId { get; set; }

        [JsonProperty("registrationType")]
        public string RegistrationType { get; set; }

        [JsonProperty("customerCountry")]
        public string CustomerCountry { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; } = "Registered";
    }

    public class BagRegistration
    {
        [JsonProperty("actionFlag")]
        public string ActionFlag { get; set; }

        [JsonProperty("customerNumber")]
        public string CustomerNumber { get; set; }

        [JsonProperty("customerName")]
        public string CustomerName { get; set; }

        [JsonProperty("bankInfo")]
        public BankInfo BankInfo { get; set; }

        [JsonProperty("customerAddress")]
        public string CustomerAddress { get; set; }

        [JsonProperty("registeredName")]
        public string RegisteredName { get; set; }

        [JsonProperty("bagNumber")]
        public string BagNumber { get; set; }

        [JsonProperty("turnoverDate")]
        public DateTime TurnoverDate { get; set; }

        [JsonProperty("referenceStatement")]
        public string ReferenceStatement { get; set; }

        [JsonProperty("exchangeRates")]
        public List<ExchangeRate> ExchangeRates { get; set; }

        [JsonProperty("registrationDate")]
        public DateTime RegistrationDate { get; set; }

        [JsonProperty("registeredBy")]
        public string RegisteredBy { get; set; }

        [JsonProperty("registeredCoins")]
        public string RegisteredCoins { get; set; }

        [JsonProperty("registeredCash")]
        public string RegisteredCash { get; set; }

        [JsonProperty("registeredChecks")]
        public string RegisteredChecks { get; set; }

        [JsonProperty("registeredForeignCurrency")]
        public string RegisteredForeignCurrency { get; set; }

        [JsonProperty("countedAmount")]
        public string CountedAmount { get; set; }

        [JsonProperty("countedDate")]
        public DateTime CountedDate { get; set; }

        [JsonProperty("totalAmount")]
        public string TotalAmount { get; set; }

        [JsonProperty("locationId")]
        public string LocationId { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("shopNumber")]
        public string ShopNumber { get; set; }

        [JsonProperty("easySafeAccount")]
        public string EasySafeAccount { get; set; }

        [JsonProperty("registrationApproval")]
        public string RegistrationApproval { get; set; }

        [JsonProperty("isBankAccount")]
        public string IsBankAccount { get; set; }

        [JsonProperty("notes")]
        public Notes Notes { get; set; }

        [JsonProperty("registrationStatus")]
        public string RegistrationStatus { get; set; }

        [JsonProperty("contracts")]
        public Contracts Contracts { get; set; }

        [JsonProperty("pickedUpDate")]
        public DateTime PickedUpDate { get; set; }

        [JsonProperty("nightSafeId")]
        public string NightSafeId { get; set; }

        [JsonProperty("registrationSubType")]
        public string RegistrationSubType { get; set; }

        [JsonProperty("foreignCurrencies")]
        public List<ForeignCurrency> ForeignCurrencies { get; set; }

        [JsonProperty("customerCountry")]
        public string CustomerCountry { get; set; }

        [JsonProperty("confirmFlag")]
        public string ConfirmFlag { get; set; }

        [JsonProperty("registeredUserId")]
        public string RegisteredUserId { get; set; }

        [JsonProperty("confirmDateTime")]
        public DateTime ConfirmDateTime { get; set; }

        [JsonProperty("dropNSName")]
        public string DropNSName { get; set; }

        [JsonProperty("dropNSDateTime")]
        public DateTime DropNSDateTime { get; set; }

        [JsonProperty("vouchers")]
        public List<VoucherDetail> Vouchers { get; set; }
    }

    public class BankInfo
    {
        [JsonProperty("actingBranchName")]
        public string ActingBranchName { get; set; }

        [JsonProperty("actingBranchId")]
        public string ActingBranchId { get; set; }

        [JsonProperty("mainBankName")]
        public string MainBankName { get; set; }

        [JsonProperty("mainBankId")]
        public string MainBankId { get; set; }
    }

    public class ExchangeRate
    {
        [JsonProperty("foreignCurrencyCode")]
        public string ForeignCurrencyCode { get; set; }

        [JsonProperty("foreignCurrencyAmt")]
        public string ForeignCurrencyAmt { get; set; }

        [JsonProperty("exchangeRate")]
        public string ExchangeRateValue { get; set; }

        [JsonProperty("amountInLocalCurrency")]
        public string AmountInLocalCurrency { get; set; }
    }

    public class Notes
    {
        [JsonProperty("seddel1000")]
        public string Seddel1000 { get; set; }

        [JsonProperty("seddel500")]
        public string Seddel500 { get; set; }

        [JsonProperty("seddel200")]
        public string Seddel200 { get; set; }

        [JsonProperty("seddel100")]
        public string Seddel100 { get; set; }

        [JsonProperty("seddel50")]
        public string Seddel50 { get; set; }
    }

    public class Contracts
    {
        [JsonProperty("isPDADeclared")]
        public string IsPDADeclared { get; set; }

        [JsonProperty("deviationType")]
        public string DeviationType { get; set; }

        [JsonProperty("declarationType")]
        public string DeclarationType { get; set; }

        [JsonProperty("containsValuta")]
        public string ContainsValuta { get; set; }

        [JsonProperty("controlledAmount")]
        public string ControlledAmount { get; set; }
    }

    public class ForeignCurrency
    {
        [JsonProperty("currencyISOCode")]
        public string CurrencyISOCode { get; set; }

        [JsonProperty("buyNotes")]
        public string BuyNotes { get; set; }

        [JsonProperty("notesAmountNok")]
        public string NotesAmountNok { get; set; }

        [JsonProperty("notesAmountCurrency")]
        public string NotesAmountCurrency { get; set; }

        [JsonProperty("buyCheques")]
        public string BuyCheques { get; set; }

        [JsonProperty("chequesAmountNOK")]
        public string ChequesAmountNOK { get; set; }

        [JsonProperty("chequesAmountCurrency")]
        public string ChequesAmountCurrency { get; set; }

        [JsonProperty("currencyAmount")]
        public string CurrencyAmount { get; set; }

        [JsonProperty("regDate")]
        public DateTime RegDate { get; set; }

        [JsonProperty("itemId")]
        public string ItemId { get; set; }

        [JsonProperty("exchangeRate")]
        public string ExchangeRate { get; set; }
    }

    public class VoucherDetail
    {
        [JsonProperty("voucherNumber")]
        public string VoucherNumber { get; set; }

        [JsonProperty("voucherAmount")]
        public string VoucherAmount { get; set; }

        [JsonProperty("bankBranchNumber")]
        public string BankBranchNumber { get; set; }

        [JsonProperty("bankBranchName")]
        public string BankBranchName { get; set; }
    }
}
