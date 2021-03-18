using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Services.Order.Commands;

namespace Sheaft.Services.Mappings
{
    public class OrderInputProfile : Profile
    {
        public OrderInputProfile()
        {
            CreateMap<CreateOrderDto, CreateConsumerOrderCommand>();
            CreateMap<CreateOrderDto, CreateBusinessOrderCommand>();

            CreateMap<UpdateOrderDto, UpdateConsumerOrderCommand>()
                .ForMember(c => c.OrderId, opt => opt.MapFrom(r => r.Id));;
            CreateMap<ResourceIdDto, PayOrderCommand>()
                .ForMember(c => c.OrderId, opt => opt.MapFrom(e => e.Id));

            CreateMap<ResourceIdDto, ConfirmOrderCommand>()
                .ForMember(c => c.OrderId, opt => opt.MapFrom(e => e.Id));
        }
    }
}
