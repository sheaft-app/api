using System.Linq;
using AutoMapper;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappings
{
    public class StoreProfile : Profile
    {
        public StoreProfile()
        {
            CreateMap<Domain.Store, UserDto>();
            
            CreateMap<Domain.Store, StoreDto>()
                .IncludeBase<Domain.Store, UserDto>()
                .ForMember(d => d.Tags, opt => opt.MapFrom(r => r.Tags.Select(t => t.Tag)));;
        }
    }
}
