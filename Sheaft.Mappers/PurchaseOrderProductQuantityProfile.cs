using AutoMapper;
using Sheaft.Domain.Models;
using Sheaft.Models.Dto;

namespace Sheaft.Mappers
{
    public class PurchaseOrderProductQuantityProfile : Profile
    {
        public PurchaseOrderProductQuantityProfile()
        {
            CreateMap<PurchaseOrderProduct, PurchaseOrderProductQuantityDto>();
        }
    }
}
