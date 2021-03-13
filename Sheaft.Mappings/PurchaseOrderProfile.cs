using AutoMapper;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Application.Common.Models.Inputs;
using Sheaft.Application.Common.Models.ViewModels;
using Sheaft.Application.PurchaseOrder.Commands;

namespace Sheaft.Mappings
{
    public class PurchaseOrderProfile : Profile
    {
        public PurchaseOrderProfile()
        {
            CreateMap<Domain.PurchaseOrder, PurchaseOrderDto>()
                .ForMember(d => d.Sender, opt => opt.MapFrom(r => r.Sender))
                .ForMember(d => d.Vendor, opt => opt.MapFrom(r => r.Vendor))
                .ForMember(d => d.ExpectedDelivery, opt => opt.MapFrom(r => r.ExpectedDelivery))
                .ForMember(d => d.Products, opt => opt.MapFrom(r => r.Products));

            CreateMap<Domain.PurchaseOrder, PurchaseOrderShortViewModel>();

            CreateMap<Domain.PurchaseOrder, PurchaseOrderViewModel>()
                .ForMember(d => d.Sender, opt => opt.MapFrom(r => r.Sender))
                .ForMember(d => d.Vendor, opt => opt.MapFrom(r => r.Vendor))
                .ForMember(d => d.Transfer, opt => opt.MapFrom(r => r.Transfer))
                .ForMember(d => d.ExpectedDelivery, opt => opt.MapFrom(r => r.ExpectedDelivery))
                .ForMember(d => d.Products, opt => opt.MapFrom(r => r.Products));

            CreateMap<CreatePurchaseOrderInput, CreatePurchaseOrderCommand>();
            CreateMap<IdsInput, AcceptPurchaseOrdersCommand>()
                .ForMember(c => c.PurchaseOrderIds, opt => opt.MapFrom(r => r.Ids));
            CreateMap<IdsInput, ShipPurchaseOrdersCommand>()
                .ForMember(c => c.PurchaseOrderIds, opt => opt.MapFrom(r => r.Ids));
            CreateMap<IdsInput, DeliverPurchaseOrdersCommand>()
                .ForMember(c => c.PurchaseOrderIds, opt => opt.MapFrom(r => r.Ids));
            CreateMap<IdsInput, ProcessPurchaseOrdersCommand>()
                .ForMember(c => c.PurchaseOrderIds, opt => opt.MapFrom(r => r.Ids));
            CreateMap<IdsInput, CompletePurchaseOrdersCommand>()
                .ForMember(c => c.PurchaseOrderIds, opt => opt.MapFrom(r => r.Ids));
            CreateMap<IdsInput, DeletePurchaseOrdersCommand>()
                .ForMember(c => c.PurchaseOrderIds, opt => opt.MapFrom(r => r.Ids));
            CreateMap<IdsWithReasonInput, CancelPurchaseOrdersCommand>()
                .ForMember(c => c.PurchaseOrderIds, opt => opt.MapFrom(r => r.Ids));
            CreateMap<IdsWithReasonInput, RefusePurchaseOrdersCommand>()
                .ForMember(c => c.PurchaseOrderIds, opt => opt.MapFrom(r => r.Ids));
        }
    }
}
