using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Mappings
{
    public class BusinessProfile : Profile
    {
        public BusinessProfile()
        {
            CreateMap<Domain.Business, UserDto>()
                .ForMember(c => c.Summary, opt => opt.MapFrom(e => e.ProfileInformation.Summary))
                .ForMember(c => c.Description, opt => opt.MapFrom(e => e.ProfileInformation.Description))
                .ForMember(c => c.Facebook, opt => opt.MapFrom(e => e.ProfileInformation.Facebook))
                .ForMember(c => c.Instagram, opt => opt.MapFrom(e => e.ProfileInformation.Instagram))
                .ForMember(c => c.Twitter, opt => opt.MapFrom(e => e.ProfileInformation.Twitter))
                .ForMember(c => c.Website, opt => opt.MapFrom(e => e.ProfileInformation.Website))
                .ForMember(c => c.Pictures, opt => opt.MapFrom(e => e.ProfileInformation.Pictures));
            
            CreateMap<Domain.Business, ProducerDto>()
                .IncludeBase<Domain.Business, UserDto>();
            
            CreateMap<Domain.Business, StoreDto>()
                .IncludeBase<Domain.Business, UserDto>();
        }
    }
}
