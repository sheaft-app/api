using AutoMapper;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class DeclarationViewProfile : Profile
    {
        public DeclarationViewProfile()
        {
            CreateMap<Domain.Declaration, DeclarationViewModel>();
        }
    }
}
