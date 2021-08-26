using AutoMapper;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class PurchaseOrderViewProfile : Profile
    {
        public PurchaseOrderViewProfile()
        {
            CreateMap<Domain.PurchaseOrder, ShortPurchaseOrderViewModel>();
            CreateMap<Domain.PurchaseOrder, PurchaseOrderViewModel>();

        }
    }
}
