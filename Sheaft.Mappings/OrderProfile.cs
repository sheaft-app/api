using AutoMapper;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Application.Common.Models.Inputs;
using Sheaft.Application.Common.Models.ViewModels;
using Sheaft.Application.Order.Commands;

namespace Sheaft.Mappings
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Domain.Order, OrderDto>()
                .ForMember(d => d.User, opt => opt.MapFrom(r => r.User))
                .ForMember(d => d.Donation, opt => opt.MapFrom(r => r.Donate))
                .ForMember(d => d.TotalFees, opt => opt.MapFrom(r => r.FeesPrice - r.InternalFeesPrice));

            CreateMap<Domain.Order, OrderViewModel>()
                .ForMember(d => d.User, opt => opt.MapFrom(r => r.User))
                .ForMember(d => d.Payin, opt => opt.MapFrom(r => r.Payin))
                .ForMember(d => d.PurchaseOrders, opt => opt.MapFrom(r => r.PurchaseOrders))
                .ForMember(d => d.TotalFees, opt => opt.MapFrom(r => r.FeesPrice - r.InternalFeesPrice));

            CreateMap<Domain.Order, OrderShortViewModel>();

            CreateMap<CreateOrderInput, CreateConsumerOrderCommand>();
            CreateMap<CreateOrderInput, CreateBusinessOrderCommand>();

            CreateMap<UpdateOrderInput, UpdateConsumerOrderCommand>()
                .ForMember(c => c.OrderId, opt => opt.MapFrom(r => r.Id));;
            CreateMap<IdInput, PayOrderCommand>()
                .ForMember(c => c.OrderId, opt => opt.MapFrom(e => e.Id));

            CreateMap<IdInput, ConfirmOrderCommand>()
                .ForMember(c => c.OrderId, opt => opt.MapFrom(e => e.Id));
        }
    }
}
