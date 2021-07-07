using AutoMapper;
using Sheaft.Domain;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class PreAuthorizationViewProfile : Profile
    {
        public PreAuthorizationViewProfile()
        {
            CreateMap<PreAuthorization, PreAuthorizationViewModel>();
        }
    }
}