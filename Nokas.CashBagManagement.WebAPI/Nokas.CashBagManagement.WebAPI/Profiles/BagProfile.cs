using AutoMapper;

namespace Nokas.CashBagManagement.WebAPI.Profiles
{
    public class BagProfile : Profile
    {
        public BagProfile()
        {
            // Map creation model to domain/response model
            CreateMap<Models.BagRegistrationForCreation, Models.BagRegistration>();

            // Optional: Add child mappings if types differ (e.g., ExchangeRateForCreation -> ExchangeRate)
            CreateMap<Models.ExchangeRateForCreation, Models.ExchangeRate>();
            CreateMap<Models.ForeignCurrencyForCreation, Models.ForeignCurrency>();
            CreateMap<Models.ContractsForCreation, Models.Contracts>();
            CreateMap<Models.NotesForCreation, Models.Notes>();
            CreateMap<Models.VoucherDetailForCreation, Models.VoucherDetail>();
            CreateMap<Models.BankInfoForCreation, Models.BankInfo>();
        }
    }
}

