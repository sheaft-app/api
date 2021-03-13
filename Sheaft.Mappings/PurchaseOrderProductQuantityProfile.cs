using AutoMapper;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Application.Common.Models.ViewModels;
using Sheaft.Domain;

namespace Sheaft.Mappings
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
