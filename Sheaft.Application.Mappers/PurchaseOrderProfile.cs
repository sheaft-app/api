using AutoMapper;
using Sheaft.Application.Commands;
using Sheaft.Domain.Models;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappers
{
    public class PurchaseOrderProfile : Profile
    {
        public PurchaseOrderProfile()
        {
            CreateMap<PurchaseOrder, PurchaseOrderDto>()
                .ForMember(d => d.Sender, opt => opt.MapFrom(r => r.Sender))
                .ForMember(d => d.Vendor, opt => opt.MapFrom(r => r.Vendor))
                .ForMember(d => d.ExpectedDelivery, opt => opt.MapFrom(r => r.ExpectedDelivery))
                .ForMember(d => d.Products, opt => opt.MapFrom(r => r.Products));

            CreateMap<PurchaseOrder, PurchaseOrderViewModel>()
                .ForMember(d => d.Sender, opt => opt.MapFrom(r => r.Sender))
                .ForMember(d => d.Vendor, opt => opt.MapFrom(r => r.Vendor))
                .ForMember(d => d.ExpectedDelivery, opt => opt.MapFrom(r => r.ExpectedDelivery))
                .ForMember(d => d.Products, opt => opt.MapFrom(r => r.Products));

            CreateMap<CreatePurchaseOrderInput, CreatePurchaseOrderCommand>();
            CreateMap<IdsInput, AcceptPurchaseOrdersCommand>();
            CreateMap<IdsInput, ShipPurchaseOrdersCommand>();
            CreateMap<IdsInput, DeliverPurchaseOrdersCommand>();
            CreateMap<IdsInput, ProcessPurchaseOrdersCommand>();
            CreateMap<IdsInput, CompletePurchaseOrdersCommand>();
            CreateMap<IdsInput, DeletePurchaseOrdersCommand>();
            CreateMap<IdsWithReasonInput, CancelPurchaseOrdersCommand>();
            CreateMap<IdsWithReasonInput, RefusePurchaseOrdersCommand>();
        }
    }
}
