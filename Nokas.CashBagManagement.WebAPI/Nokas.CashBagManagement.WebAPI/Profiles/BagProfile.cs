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
            CreateMap<Models.BagRegistration, Models.BagRegistration>();
            CreateMap<Models.ExchangeRate, Models.ExchangeRate>();
            CreateMap<Models.ForeignCurrency, Models.ForeignCurrency>();
            CreateMap<Models.Contracts, Models.Contracts>();
            CreateMap<Models.Notes, Models.Notes>();
            CreateMap<Models.VoucherDetail, Models.VoucherDetail>();
            CreateMap<Models.BankInfo, Models.BankInfo>();

            // Cosmos/SQL ↔ Domain
            CreateMap<BagRegistrationRequest, BagRegistrationDetailsResponse>();
        }
    }
}
