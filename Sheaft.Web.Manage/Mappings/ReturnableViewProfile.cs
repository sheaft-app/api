using AutoMapper;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class ReturnableViewProfile : Profile
    {
        public ReturnableViewProfile()
        {
            CreateMap<Domain.Returnable, ReturnableViewModel>();
        }
    }
}
