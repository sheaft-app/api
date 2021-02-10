using AutoMapper;
using Sheaft.Application.Commands;
using Sheaft.Domain.Models;
using Sheaft.Application.Models;
using System.Linq;

namespace Sheaft.Application.Mappers
{
    public class DeclarationProfile : Profile
    {
        public DeclarationProfile()
        {
            CreateMap<Declaration, DeclarationDto>()
                .ForMember(c => c.Ubos, opt => opt.MapFrom(e => e.Ubos));

            CreateMap<Declaration, DeclarationViewModel>()
                .ForMember(c => c.Ubos, opt => opt.MapFrom(e => e.Ubos));
        }
    }
}
