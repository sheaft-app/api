using AutoMapper;
using Sheaft.Application.Commands;
using Sheaft.Domain.Models;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappers
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDto>()
                .ForMember(d => d.User, opt => opt.MapFrom(r => r.User))
                .ForMember(d => d.Donation, opt => opt.MapFrom(r => r.Donate))
                .ForMember(d => d.TotalFees, opt => opt.MapFrom(r => r.FeesPrice - r.InternalFeesPrice));

            CreateMap<Order, OrderViewModel>()
                .ForMember(d => d.User, opt => opt.MapFrom(r => r.User))
                .ForMember(d => d.Payin, opt => opt.MapFrom(r => r.Payin))
                .ForMember(d => d.PurchaseOrders, opt => opt.MapFrom(r => r.PurchaseOrders))
                .ForMember(d => d.TotalFees, opt => opt.MapFrom(r => r.FeesPrice - r.InternalFeesPrice));

            CreateMap<Order, OrderShortViewModel>();

            CreateMap<CreateOrderInput, CreateConsumerOrderCommand>();
            CreateMap<CreateOrderInput, CreateBusinessOrderCommand>();

            CreateMap<UpdateOrderInput, UpdateConsumerOrderCommand>();
            CreateMap<IdInput, ConfirmOrderCommand>()
                .ForMember(c => c.OrderId, opt => opt.MapFrom(e => e.Id));
        }
    }
}
