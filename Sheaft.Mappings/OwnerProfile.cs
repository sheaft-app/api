using AutoMapper;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Application.Common.Models.Inputs;
using Sheaft.Application.Common.Models.ViewModels;
using Sheaft.Domain;

namespace Sheaft.Mappings
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
