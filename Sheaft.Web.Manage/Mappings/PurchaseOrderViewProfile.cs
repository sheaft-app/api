using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class PurchaseOrderViewProfile : Profile
    {
        public PurchaseOrderViewProfile()
        {
            CreateMap<Domain.PurchaseOrder, PurchaseOrderShortViewModel>();
            CreateMap<Domain.PurchaseOrder, PurchaseOrderViewModel>();
            CreateMap<PurchaseOrderDto, PurchaseOrderViewModel>();
            CreateMap<PurchaseOrderDto, PurchaseOrderShortViewModel>();
            CreateMap<PurchaseOrderViewModel, PurchaseOrderDto>();
            CreateMap<PurchaseOrderShortViewModel, PurchaseOrderDto>();

        }
    }
}
