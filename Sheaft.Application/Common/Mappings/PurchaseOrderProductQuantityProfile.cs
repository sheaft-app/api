using AutoMapper;
using Sheaft.Domain.Models;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappers
{
    public class PurchaseOrderProductQuantityProfile : Profile
    {
        public PurchaseOrderProductQuantityProfile()
        {
            CreateMap<PurchaseOrderProduct, PurchaseOrderProductQuantityDto>();
            CreateMap<PurchaseOrderProduct, PurchaseOrderProductViewModel>();
        }
    }
}
