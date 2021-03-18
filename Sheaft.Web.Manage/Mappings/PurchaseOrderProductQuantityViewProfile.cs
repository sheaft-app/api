using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Domain;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class PurchaseOrderProductQuantityViewProfile : Profile
    {
        public PurchaseOrderProductQuantityViewProfile()
        {
            CreateMap<PurchaseOrderProduct, PurchaseOrderProductViewModel>();
            CreateMap<PurchaseOrderProductQuantityDto, PurchaseOrderProductViewModel>();
            CreateMap<PurchaseOrderProductViewModel, PurchaseOrderProductQuantityDto>();
        }
    }
}
