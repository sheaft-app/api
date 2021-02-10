using AutoMapper;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Application.Common.Models.ViewModels;
using Sheaft.Domain;

namespace Sheaft.Application.Common.Mappings
{
    public class BusinessProfile : Profile
    {
        public BusinessProfile()
        {
            CreateMap<Business, UserViewModel>();

            CreateMap<Business, UserProfileDto>();

            CreateMap<Business, UserDto>()
                .ForMember(d => d.Address, opt => opt.MapFrom(r => r.Address));

            CreateMap<Business, BusinessProfileDto>()
                .IncludeBase<Business, UserDto>();
        }
    }
}
