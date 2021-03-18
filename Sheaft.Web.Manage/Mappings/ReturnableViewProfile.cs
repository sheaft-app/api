using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class ReturnableViewProfile : Profile
    {
        public ReturnableViewProfile()
        {
            CreateMap<Domain.Returnable, ReturnableViewModel>();
            CreateMap<ReturnableDto, ReturnableViewModel>();
            CreateMap<ReturnableViewModel, ReturnableDto>();
        }
    }
}
