using AutoMapper;
using Sheaft.Application.Commands;
using Sheaft.Domain.Models;
using Sheaft.Models.Dto;
using Sheaft.Models.Inputs;
using Sheaft.Models.ViewModels;

namespace Sheaft.Mappers
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDto>()
                .ForMember(d => d.User, opt => opt.MapFrom(r => r.User));

            CreateMap<CreateOrderInput, CreateOrderCommand>();
            CreateMap<PayOrderInput, PayOrderCommand>();
            CreateMap<ConfirmOrderInput, ConfirmOrderCommand>();
        }
    }
}
