using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Domain;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class ExpectedPurchaseOrderDeliveryViewProfile : Profile
    {
        public ExpectedPurchaseOrderDeliveryViewProfile()
        {
            CreateMap<ExpectedPurchaseOrderDelivery, ExpectedPurchaseOrderDeliveryViewModel>();
            CreateMap<ExpectedPurchaseOrderDeliveryDto, ExpectedPurchaseOrderDeliveryViewModel>();
            CreateMap<ExpectedPurchaseOrderDeliveryViewModel, ExpectedPurchaseOrderDeliveryDto>();
        }
    }
}
