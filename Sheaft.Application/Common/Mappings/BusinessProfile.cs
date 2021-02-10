using AutoMapper;
using Sheaft.Application.Commands;
using Sheaft.Domain.Models;
using Sheaft.Application.Models;
using System.Linq;

namespace Sheaft.Application.Mappers
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
