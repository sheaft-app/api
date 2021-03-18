using AutoMapper;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappings
{
    public class OrderDeliveryProfile : Profile
    {
        public OrderDeliveryProfile()
        {
            CreateMap<Domain.OrderDelivery, OrderDeliveryDto>();
        }
    }
}