using AutoMapper;
using Sheaft.Domain;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class ExpectedPurchaseOrderDeliveryViewProfile : Profile
    {
        public ExpectedPurchaseOrderDeliveryViewProfile()
        {
            CreateMap<ExpectedPurchaseOrderDelivery, ExpectedPurchaseOrderDeliveryViewModel>();
        }
    }
}
