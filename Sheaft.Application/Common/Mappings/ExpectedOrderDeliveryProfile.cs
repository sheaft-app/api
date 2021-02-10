using AutoMapper;
using Sheaft.Domain.Models;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappers
{
    public class ExpectedOrderDeliveryProfile : Profile
    {
        public ExpectedOrderDeliveryProfile()
        {
            CreateMap<ExpectedOrderDelivery, ExpectedOrderDeliveryDto>()
                 .ForMember(d => d.Day, opt => opt.MapFrom(r => r.ExpectedDeliveryDate.DayOfWeek));

            CreateMap<ExpectedOrderDelivery, ExpectedOrderDeliveryViewModel>()
                 .ForMember(d => d.Day, opt => opt.MapFrom(r => r.ExpectedDeliveryDate.DayOfWeek));
        }
    }
}
