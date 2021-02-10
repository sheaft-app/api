using AutoMapper;
using Sheaft.Domain.Models;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappers
{
    public class OwnerProfile : Profile
    {
        public OwnerProfile()
        {
            CreateMap<Owner, OwnerDto>();
            CreateMap<Owner, OwnerViewModel>();

            CreateMap<OwnerViewModel, OwnerInput>()
                .ForMember(d => d.Address, opt => opt.MapFrom(r => r.Address));
        }
    }
}
