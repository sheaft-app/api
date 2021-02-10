using AutoMapper;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Application.Common.Models.ViewModels;

namespace Sheaft.Application.Common.Mappings
{
    public class DeclarationProfile : Profile
    {
        public DeclarationProfile()
        {
            CreateMap<Domain.Declaration, DeclarationDto>()
                .ForMember(c => c.Ubos, opt => opt.MapFrom(e => e.Ubos));

            CreateMap<Domain.Declaration, DeclarationViewModel>()
                .ForMember(c => c.Ubos, opt => opt.MapFrom(e => e.Ubos));
        }
    }
}
