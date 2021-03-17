using AutoMapper;
using Sheaft.Application.Common.Models.Dto;

namespace Sheaft.Mappings
{
    public class OrderDeliveryProfile : Profile
    {
        public OrderDeliveryProfile()
        {
            CreateMap<Domain.OrderDelivery, OrderDeliveryDto>();
        }
    }
}