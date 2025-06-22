using AutoMapper;
using Nokas.CashBagManagement.WebAPI.Entities;
using Nokas.CashBagManagement.WebAPI.Models;

namespace Nokas.CashBagManagement.WebAPI.Profiles
{
    public class BagProfile : Profile
    {
        public BagProfile()
        {


            // ✅ SQL Entity <-> Model mappings
            CreateMap<BagRegistrationEntity, BagRegistrationRequest>().ReverseMap();
            CreateMap<Entities.BagRegistration, Models.BagRegistration>().ReverseMap();
            CreateMap<Entities.BankInfo, Models.BankInfo>().ReverseMap();
            CreateMap<Entities.ExchangeRate, Models.ExchangeRate>().ReverseMap();
            CreateMap<Entities.Notes, Models.Notes>().ReverseMap();
            CreateMap<Entities.Contracts, Models.Contracts>().ReverseMap();
            CreateMap<Entities.ForeignCurrency, Models.ForeignCurrency>().ReverseMap();
            CreateMap<Entities.VoucherDetail, Models.VoucherDetail>().ReverseMap();

            // ✅ DTO (ForCreation) → Model mappings
            CreateMap<BagRegistrationRequestForCreation, BagRegistrationRequest>();
            CreateMap<Models.BagRegistrationForCreation, Models.BagRegistration>();
            CreateMap<Models.ExchangeRateForCreation, Models.ExchangeRate>();
            CreateMap<Models.ForeignCurrencyForCreation, Models.ForeignCurrency>();
            CreateMap<Models.ContractsForCreation, Models.Contracts>();
            CreateMap<Models.NotesForCreation, Models.Notes>();
            CreateMap<Models.VoucherDetailForCreation, Models.VoucherDetail>();
            CreateMap<Models.BankInfoForCreation, Models.BankInfo>();
        }
    }
}
