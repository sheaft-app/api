using AutoMapper;
using Sheaft.Domain.Models;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappers
{
    public class ExpectedPurchaseOrderDeliveryProfile : Profile
    {
        public ExpectedPurchaseOrderDeliveryProfile()
        {
            CreateMap<ExpectedPurchaseOrderDelivery, ExpectedPurchaseOrderDeliveryDto>()
                 .ForMember(d => d.Address, opt => opt.MapFrom(r => r.Address))
                 .ForMember(d => d.Day, opt => opt.MapFrom(r => r.ExpectedDeliveryDate.DayOfWeek));

            CreateMap<ExpectedPurchaseOrderDelivery, ExpectedPurchaseOrderDeliveryViewModel>()
                 .ForMember(d => d.Address, opt => opt.MapFrom(r => r.Address))
                 .ForMember(d => d.Day, opt => opt.MapFrom(r => r.ExpectedDeliveryDate.DayOfWeek));
        }
    }
}
