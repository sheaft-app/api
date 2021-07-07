using AutoMapper;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class DeliveryProductViewProfile : Profile
    {
        public DeliveryProductViewProfile()
        {
            CreateMap<Domain.DeliveryProduct, DeliveryProductViewModel>();
        }
    }
}