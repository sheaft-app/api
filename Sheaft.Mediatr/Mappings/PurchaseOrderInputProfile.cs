using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Mediatr.PurchaseOrder.Commands;

namespace Sheaft.Mediatr.Mappings
{
    public class PurchaseOrderInputProfile : Profile
    {
        public PurchaseOrderInputProfile()
        {
            CreateMap<CreatePurchaseOrderDto, CreatePurchaseOrderCommand>();
            CreateMap<ResourceIdsDto, AcceptPurchaseOrdersCommand>()
                .ForMember(c => c.PurchaseOrderIds, opt => opt.MapFrom(r => r.Ids));
            CreateMap<ResourceIdsDto, ShipPurchaseOrdersCommand>()
                .ForMember(c => c.PurchaseOrderIds, opt => opt.MapFrom(r => r.Ids));
            CreateMap<ResourceIdsDto, DeliverPurchaseOrdersCommand>()
                .ForMember(c => c.PurchaseOrderIds, opt => opt.MapFrom(r => r.Ids));
            CreateMap<ResourceIdsDto, ProcessPurchaseOrdersCommand>()
                .ForMember(c => c.PurchaseOrderIds, opt => opt.MapFrom(r => r.Ids));
            CreateMap<ResourceIdsDto, CompletePurchaseOrdersCommand>()
                .ForMember(c => c.PurchaseOrderIds, opt => opt.MapFrom(r => r.Ids));
            CreateMap<ResourceIdsDto, DeletePurchaseOrdersCommand>()
                .ForMember(c => c.PurchaseOrderIds, opt => opt.MapFrom(r => r.Ids));
            CreateMap<ResourceIdsWithReasonDto, CancelPurchaseOrdersCommand>()
                .ForMember(c => c.PurchaseOrderIds, opt => opt.MapFrom(r => r.Ids));
            CreateMap<ResourceIdsWithReasonDto, RefusePurchaseOrdersCommand>()
                .ForMember(c => c.PurchaseOrderIds, opt => opt.MapFrom(r => r.Ids));
            CreateMap<ResourceIdWithReasonDto, WithdrawnPurchaseOrderCommand>()
                .ForMember(c => c.PurchaseOrderId, opt => opt.MapFrom(r => r.Id));
        }
    }
}
