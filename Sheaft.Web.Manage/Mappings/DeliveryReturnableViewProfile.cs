using AutoMapper;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class DeliveryReturnableViewProfile : Profile
    {
        public DeliveryReturnableViewProfile()
        {
            CreateMap<Domain.DeliveryReturnable, DeliveryReturnableViewModel>();
        }
    }
}