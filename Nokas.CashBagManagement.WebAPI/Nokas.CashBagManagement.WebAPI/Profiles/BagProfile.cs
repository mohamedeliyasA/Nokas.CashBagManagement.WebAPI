using AutoMapper;
using Nokas.CashBagManagement.WebAPI.Entities;
using Nokas.CashBagManagement.WebAPI.Models;

namespace Nokas.CashBagManagement.WebAPI.Profiles
{
    public class BagProfile : Profile
    {
        public BagProfile()
        {
            // DTO (ForCreation) → Model mappings
            CreateMap<BagRegistrationRequestForCreation, BagRegistrationRequest>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.BagRegistration != null ? src.BagRegistration.BagNumber : null))
                .ForMember(dest => dest.BagRegistration, opt => opt.MapFrom(src => src.BagRegistration))
                .ForMember(dest => dest.RegistrationType, opt => opt.MapFrom(src => src.RegistrationType))
                .ForMember(dest => dest.CustomerCountry, opt => opt.MapFrom(src => src.CustomerCountry))
                .ForMember(dest => dest.RegistrationStatus, opt => opt.MapFrom(_ => "In-Progress"))
                .ForMember(dest => dest.ClientId, opt => opt.Ignore())
                .ForMember(dest => dest.CorrelationId, opt => opt.Ignore());

            // Model → DTO (For returning to client)
            CreateMap<BagRegistrationRequest, BagRegistrationRequestForCreation>()
                .ForMember(dest => dest.BagRegistration, opt => opt.MapFrom(src => src.BagRegistration))
                .ForMember(dest => dest.RegistrationType, opt => opt.MapFrom(src => src.RegistrationType))
                .ForMember(dest => dest.CustomerCountry, opt => opt.MapFrom(src => src.CustomerCountry));

            // Internal model → Summary DTO
            CreateMap<BagRegistrationRequest, BagRegSummaryResponse>()
                .ForMember(dest => dest.CustomerNumber, opt => opt.MapFrom(src => src.BagRegistration.CustomerNumber))
                .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.BagRegistration.CustomerName))
                .ForMember(dest => dest.BagNumber, opt => opt.MapFrom(src => src.BagRegistration.BagNumber))
                .ForMember(dest => dest.ActionFlag, opt => opt.MapFrom(src => src.BagRegistration.ActionFlag))
                .ForMember(dest => dest.RegistrationStatus, opt => opt.MapFrom(src => src.RegistrationStatus))
                .ForMember(dest => dest.DownstreamSystemId, opt => opt.MapFrom(src => src.DownstreamSystemId))
                .ForMember(dest => dest.BagLifecycleId, opt => opt.MapFrom(src => src.BagLifecycleId))
                .ForMember(dest => dest.RequestCorrelationId, opt => opt.Ignore()) // set manually in controller
                .ForMember(dest => dest.Description, opt => opt.Ignore());         // set manually in controller

            //  SQL Entity <-> Model mappings -> Not in use now
            CreateMap<BagRegistrationEntity, BagRegistrationRequest>().ReverseMap();
            CreateMap<Entities.BagRegistration, Models.BagRegistration>().ReverseMap();
        }
    }
}
