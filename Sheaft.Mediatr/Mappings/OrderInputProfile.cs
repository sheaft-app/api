﻿using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Mediatr.Order.Commands;

namespace Sheaft.Mediatr.Mappings
{
    public class OrderInputProfile : Profile
    {
        public OrderInputProfile()
        {
            CreateMap<CreateOrderDto, CreateConsumerOrderCommand>();
            CreateMap<CreateOrderDto, CreateBusinessOrderCommand>();

            CreateMap<UpdateOrderDto, UpdateConsumerOrderCommand>()
                .ForMember(c => c.OrderId, opt => opt.MapFrom(r => r.Id));;
            CreateMap<PayOrderDto, PayOrderCommand>()
                .ForMember(c => c.OrderId, opt => opt.MapFrom(e => e.Id));

            CreateMap<ResourceIdDto, ConfirmOrderCommand>()
                .ForMember(c => c.OrderId, opt => opt.MapFrom(e => e.Id));
        }
    }
}
