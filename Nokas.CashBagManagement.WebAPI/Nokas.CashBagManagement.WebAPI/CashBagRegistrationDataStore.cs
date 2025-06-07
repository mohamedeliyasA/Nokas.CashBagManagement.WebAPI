using Nokas.CashBagManagement.WebAPI.Models;

namespace Nokas.CashBagManagement.WebAPI
{
    public class CashBagRegistrationDataStore
    {
        public CashBagRegistrationDto CashBagRegistrationDto { get; set; }
        public static CashBagRegistrationDataStore Current { get; } = new CashBagRegistrationDataStore();



        public CashBagRegistrationDataStore()
        {
            CashBagRegistrationDto = new CashBagRegistrationDto()
            {
                Registrations = new Registration()
                {
                    ActionFlag = "A",
                    CustomerNumber = "123456",
                    CustomerAddress = "123 Main St",
                    BagNumber = "BAG123",
                    TurnoverDate = DateTime.Now,
                    ReferenceStatement = "Ref123",
                    ExchangeRates = new ExchangeRates() { ExchangeRate = "1.0" },
                    RegistrationDate = DateTime.Now,
                    RegisteredBy = "User123",
                    RegisteredCoins = 100.00m,
                    RegisteredCash = 200.00m,
                    RegisteredChecks = 50.00m,
                    RegisteredForeignCurrency = 30.00m,
                    TotalAmount = 380.00m,
                    LocationId = "LOC123",
                    ShopNumber = "SHOP123",
                    EasySafeAccount = "EASY123",
                    RegistrationApproval = 1,
                    Notes = new Notes()
                    {
                        Seddel1000 = "Note1000",
                        Seddel500 = "Note500",
                        Seddel200 = "Note200",
                        Seddel100 = "Note100",
                        Seddel50 = "Note50"
                    },
                    Contracts = new Contracts() { ContainsValuta = true },
                    NightSafeId = "NIGHT123",
                    RegistrationSubType = "SubType1",
                    ForeignCurrencies = "USD, EUR",
                    ConfirmFlag = "Y",
                    RegisteredUserId = "User123",
                    Vouchers = new List<Vouchers>()
                    { 
                        new Vouchers { VoucherDetails = "1"},
                        new Vouchers { VoucherDetails = "2"},

                        }
                },
                CacheDbRegistrationId = 1,
                CustomerCountry = null
            };
        }



    }
}
