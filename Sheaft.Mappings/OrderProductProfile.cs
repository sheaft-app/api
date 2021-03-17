using AutoMapper;
using Sheaft.Application.Common.Models.Dto;

namespace Sheaft.Mappings
{
    public class OrderProductProfile : Profile
    {
        public OrderProductProfile()
        {
            CreateMap<Domain.OrderProduct, OrderProductDto>();
        }
    }
}