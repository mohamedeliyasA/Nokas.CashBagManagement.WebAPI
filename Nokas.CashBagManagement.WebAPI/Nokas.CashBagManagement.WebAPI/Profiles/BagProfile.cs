using AutoMapper;
using Nokas.CashBagManagement.WebAPI.Entities;
using Nokas.CashBagManagement.WebAPI.Models;

namespace Nokas.CashBagManagement.WebAPI.Profiles
{
    public class BagProfile : Profile
    {
        public BagProfile()
        {


            // SQL Entity <-> Model mappings
            CreateMap<BagRegistrationEntity, BagRegistrationRequest>().ReverseMap();
            CreateMap<Entities.BagRegistration, Models.BagRegistration>().ReverseMap();
            CreateMap<Entities.BankInfo, Models.BankInfo>().ReverseMap();
            CreateMap<Entities.ExchangeRate, Models.ExchangeRate>().ReverseMap();
            CreateMap<Entities.Notes, Models.Notes>().ReverseMap();
            CreateMap<Entities.Contracts, Models.Contracts>().ReverseMap();
            CreateMap<Entities.ForeignCurrency, Models.ForeignCurrency>().ReverseMap();
            CreateMap<Entities.VoucherDetail, Models.VoucherDetail>().ReverseMap();

            // DTO (ForCreation) → Model mappings
            CreateMap<BagRegistrationRequestForCreation, BagRegistrationRequest>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.BagRegistration != null ? src.BagRegistration.BagNumber : null))
                .ForMember(dest => dest.BagRegistration, opt => opt.MapFrom(src => src.BagRegistration))
                .ForMember(dest => dest.CacheDbRegistrationId, opt => opt.MapFrom(src => src.CacheDbRegistrationId))
                .ForMember(dest => dest.RegistrationType, opt => opt.MapFrom(src => src.RegistrationType))
                .ForMember(dest => dest.CustomerCountry, opt => opt.MapFrom(src => src.CustomerCountry))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => "In-Progress"))
                .ForMember(dest => dest.ClientId, opt => opt.Ignore())
                .ForMember(dest => dest.CorrelationId, opt => opt.Ignore());

            // Cosmos ↔ Domain
            CreateMap<BagRegistrationRequest, BagRegSummaryResponse>()
                .ForMember(dest => dest.CustomerNumber, opt => opt.MapFrom(src => src.BagRegistration.CustomerNumber))
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.BagRegistration.CustomerName))
                .ForMember(dest => dest.BagNumber, opt => opt.MapFrom(src => src.BagRegistration.BagNumber))
                .ForMember(dest => dest.ActionFlag, opt => opt.MapFrom(src => src.BagRegistration.ActionFlag))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.CacheDbRegistrationId, opt => opt.MapFrom(src => src.CacheDbRegistrationId))
                .ForMember(dest => dest.CorrelationId, opt => opt.MapFrom(src => src.CorrelationId));

            CreateMap<BagRegistrationRequest, BagRegSummaryResponse>()
    .ForMember(dest => dest.CustomerNumber, opt => opt.MapFrom(src => src.BagRegistration.CustomerNumber))
    .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.BagRegistration.CustomerName))
    .ForMember(dest => dest.BagNumber, opt => opt.MapFrom(src => src.BagRegistration.BagNumber))
    .ForMember(dest => dest.ActionFlag, opt => opt.MapFrom(src => src.BagRegistration.ActionFlag))
    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
    .ForMember(dest => dest.CacheDbRegistrationId, opt => opt.MapFrom(src => src.CacheDbRegistrationId))
    .ForMember(dest => dest.CorrelationId, opt => opt.MapFrom(src => src.CorrelationId))
    .ForMember(dest => dest.RequestCorrelationId, opt => opt.Ignore()) // set manually
    .ForMember(dest => dest.Description, opt => opt.Ignore());         // set manually


        }
    }
}
