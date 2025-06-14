using System;
using System.Collections.Generic;
using Nokas.CashBagManagement.WebAPI.Models;

namespace Nokas.CashBagManagement.WebAPI
{
    public class BagRegistrationDataStore
    {
        public BagRegistrationRequest BagRegistrationRequest { get; set; }

        public BagRegistrationDataStore()
        {
            BagRegistrationRequest = new BagRegistrationRequest
            {
                CacheDbRegistrationId = "REG001",
                RegistrationType = "Standard",
                CustomerCountry = "Norway",

                BagRegistration = new BagRegistration
                {
                    ActionFlag = "A",
                    CustomerNumber = "123456",
                    CustomerName = "ABC Corp",
                    CustomerAddress = "123 Main St, Oslo",
                    RegisteredName = "ABC Corp",
                    BagNumber = "BAG123",
                    TurnoverDate = DateTime.Today.AddDays(-1),
                    ReferenceStatement = "Ref123",

                    ExchangeRates = new List<ExchangeRate>
                    {
                        new ExchangeRate
                        {
                            ForeignCurrencyCode = "USD",
                            ForeignCurrencyAmt = "100",
                            ExchangeRateValue = "10",
                            AmountInLocalCurrency = "1000"
                        },
                        new ExchangeRate
                        {
                            ForeignCurrencyCode = "EUR",
                            ForeignCurrencyAmt = "200",
                            ExchangeRateValue = "11",
                            AmountInLocalCurrency = "2200"
                        }
                    },

                    RegistrationDate = DateTime.Today,
                    RegisteredBy = "user123",
                    RegisteredCoins = "100.00",
                    RegisteredCash = "200.00",
                    RegisteredChecks = "50.00",
                    RegisteredForeignCurrency = "30.00",
                    CountedAmount = "380.00",
                    CountedDate = DateTime.Today,
                    TotalAmount = "380.00",
                    LocationId = "LOC123",
                    Location = "Main Office",
                    ShopNumber = "SHOP123",
                    EasySafeAccount = "EASY123",
                    RegistrationApproval = "Approved",
                    IsBankAccount = "Yes",
                    RegistrationStatus = "Processed",

                    BankInfo = new BankInfo
                    {
                        ActingBranchName = "Oslo Branch",
                        ActingBranchId = "AB001",
                        MainBankName = "MainBank",
                        MainBankId = "MB001"
                    },

                    Notes = new Notes
                    {
                        Seddel1000 = "2",
                        Seddel500 = "5",
                        Seddel200 = "10",
                        Seddel100 = "20",
                        Seddel50 = "40"
                    },

                    Contracts = new Contracts
                    {
                        IsPDADeclared = "Y",
                        DeviationType = "None",
                        DeclarationType = "Full",
                        ContainsValuta = "true",
                        ControlledAmount = "1000"
                    },

                    PickedUpDate = DateTime.Today.AddDays(1),
                    NightSafeId = "NS001",
                    RegistrationSubType = "Manual",

                    ForeignCurrencies = new List<ForeignCurrency>
                    {
                        new ForeignCurrency
                        {
                            CurrencyISOCode = "EUR",
                            BuyNotes = "50",
                            NotesAmountNok = "500",
                            NotesAmountCurrency = "50",
                            BuyCheques = "5",
                            ChequesAmountNOK = "100",
                            ChequesAmountCurrency = "10",
                            CurrencyAmount = "60",
                            RegDate = DateTime.Today,
                            ItemId = "ITEM001",
                            ExchangeRate = "10"
                        }
                    },

                    CustomerCountry = "Norway",
                    ConfirmFlag = "Y",
                    RegisteredUserId = "user123",
                    ConfirmDateTime = DateTime.Now,
                    DropNSName = "NightSafe01",
                    DropNSDateTime = DateTime.Now,

                    Vouchers = new List<VoucherDetail>
                    {
                        new VoucherDetail
                        {
                            VoucherNumber = "VCH001",
                            VoucherAmount = "100",
                            BankBranchNumber = "001",
                            BankBranchName = "Main Branch"
                        },
                        new VoucherDetail
                        {
                            VoucherNumber = "VCH002",
                            VoucherAmount = "200",
                            BankBranchNumber = "002",
                            BankBranchName = "Sub Branch"
                        }
                    }
                }
            };
        }
    }
}
