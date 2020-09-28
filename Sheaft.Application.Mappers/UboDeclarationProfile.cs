using AutoMapper;
using Sheaft.Application.Commands;
using Sheaft.Domain.Models;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappers
{
    public class UboDeclarationProfile : Profile
    {
        public UboDeclarationProfile()
        {
            CreateMap<UboDeclaration, UboDeclarationDto>();
            CreateMap<UboDeclaration, UboDeclarationViewModel>()
                .ForMember(c => c.Ubos, opt => opt.MapFrom(e => e.Ubos));
        }
    }
}
