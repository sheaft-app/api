using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class ConsumerViewProfile : Profile
    {
        public ConsumerViewProfile()
        {
            CreateMap<Domain.Consumer, UserViewModel>()
                .ForMember(c => c.Summary, opt => opt.MapFrom(e => e.ProfileInformation.Summary))
                .ForMember(c => c.Description, opt => opt.MapFrom(e => e.ProfileInformation.Description))
                .ForMember(c => c.Facebook, opt => opt.MapFrom(e => e.ProfileInformation.Facebook))
                .ForMember(c => c.Instagram, opt => opt.MapFrom(e => e.ProfileInformation.Instagram))
                .ForMember(c => c.Twitter, opt => opt.MapFrom(e => e.ProfileInformation.Twitter))
                .ForMember(c => c.Website, opt => opt.MapFrom(e => e.ProfileInformation.Website))
                .ForMember(c => c.Pictures, opt => opt.MapFrom(e => e.ProfileInformation.Pictures));
            
            CreateMap<Domain.Consumer, ConsumerViewModel>()
                .IncludeBase<Domain.Consumer, UserViewModel>();
            
            CreateMap<ConsumerDto, UserViewModel>();
            CreateMap<ConsumerDto, ConsumerViewModel>();
            CreateMap<ConsumerViewModel, ConsumerDto>();
            CreateMap<ConsumerViewModel, UserDto>();
        }
    }
}
