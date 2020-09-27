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
                .ForMember(d => d.TotalFees, opt => opt.MapFrom(r => r.FeesPrice - r.InternalFeesPrice));

            CreateMap<CreateOrderInput, CreateConsumerOrderCommand>();
            CreateMap<CreateOrderInput, CreateBusinessOrderCommand>();

            CreateMap<UpdateOrderInput, UpdateConsumerOrderCommand>();
            CreateMap<IdInput, PayOrderCommand>();
            CreateMap<IdInput, ConfirmOrderCommand>();
        }
    }
}
