using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class DeclarationViewProfile : Profile
    {
        public DeclarationViewProfile()
        {
            CreateMap<Domain.Declaration, DeclarationViewModel>();
            CreateMap<DeclarationDto, DeclarationViewModel>();
            CreateMap<DeclarationViewModel, DeclarationDto>();
        }
    }
}
