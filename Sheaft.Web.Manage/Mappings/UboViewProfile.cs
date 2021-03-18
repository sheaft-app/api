using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class UboViewProfile : Profile
    {
        public UboViewProfile()
        {
            CreateMap<Domain.Ubo, UboViewModel>();
            CreateMap<UboViewModel, UboDto>();
            CreateMap<UboDto, UboViewModel>();
        }
    }
}
