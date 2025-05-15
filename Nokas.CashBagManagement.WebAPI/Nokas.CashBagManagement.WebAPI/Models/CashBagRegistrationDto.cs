using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using static Azure.Core.HttpHeader;

namespace Nokas.CashBagManagement.WebAPI.Models
{
    public class CashBagRegistrationDto
    {
  
        public Registration Registrations { get; set; } = new Registration();

        public int CacheDbRegistrationId { get; set; }


        public string? CustomerCountry { get; set; }
    }

    public class Registration       
    {
        public string ActionFlag { get; set; } = string.Empty;
        public string CustomerNumber { get; set; } = string.Empty;
        public string CustomerAddress { get; set; } = string.Empty;
        public string BagNumber { get; set; } = string.Empty;
        public DateTime TurnoverDate { get; set; }
        public string ReferenceStatement { get; set; } = string.Empty;
        public ExchangeRates ExchangeRates { get; set; } = new();
        public DateTime RegistrationDate { get; set; }
        public string RegisteredBy { get; set; } = string.Empty;
        public decimal RegisteredCoins { get; set; }
        public decimal RegisteredCash { get; set; }
        public decimal RegisteredChecks { get; set; }
        public decimal RegisteredForeignCurrency { get; set; }
        public decimal TotalAmount { get; set; }
        public string LocationId { get; set; } = string.Empty;
        public string ShopNumber { get; set; } = string.Empty;
        public string EasySafeAccount { get; set; } = string.Empty;
        public int RegistrationApproval { get; set; }
        public Notes Notes { get; set; } = new();
        public Contracts Contracts { get; set; } = new();
        public string NightSafeId { get; set; } = string.Empty;
        public string RegistrationSubType { get; set; } = string.Empty;
        public string ForeignCurrencies { get; set; } = string.Empty;
        public string ConfirmFlag { get; set; } = string.Empty;
        public string RegisteredUserId { get; set; } = string.Empty;
        public Vouchers Vouchers { get; set; } = new();
    }

    public class ExchangeRates
    {
        public string ExchangeRate { get; set; } = string.Empty;
    }

    public class Notes
    {
        public string Seddel1000 { get; set; } = string.Empty;
        public string Seddel500 { get; set; } = string.Empty;
        public string Seddel200 { get; set; } = string.Empty;
        public string Seddel100 { get; set; } = string.Empty;
        public string Seddel50 { get; set; } = string.Empty;
    }

    public class Contracts
    {
        public bool ContainsValuta { get; set; }
    }

    public class Vouchers
    {
        public string VoucherDetails { get; set; } = string.Empty;
    }
}
