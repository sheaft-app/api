using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Mappings
{
    public class PurchaseOrderProductQuantityProfile : Profile
    {
        public PurchaseOrderProductQuantityProfile()
        {
            CreateMap<PurchaseOrderProduct, PurchaseOrderProductQuantityDto>();
        }
    }
}
