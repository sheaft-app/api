using AutoMapper;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class DeliveryViewProfile : Profile
    {
        public DeliveryViewProfile()
        {
            CreateMap<Domain.Delivery, DeliveryViewModel>();
            CreateMap<Domain.Delivery, ShortDeliveryViewModel>();
        }
    }
}