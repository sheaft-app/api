using AutoMapper;
using Sheaft.Domain.Models;
using Sheaft.Models.Dto;
using Sheaft.Models.ViewModels;

namespace Sheaft.Mappers
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
