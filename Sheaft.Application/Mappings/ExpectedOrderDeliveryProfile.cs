using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Mappings
{
    public class ExpectedOrderDeliveryProfile : Profile
    {
        public ExpectedOrderDeliveryProfile()
        {
            CreateMap<ExpectedOrderDelivery, ExpectedOrderDeliveryDto>()
                 .ForMember(d => d.Day, opt => opt.MapFrom(r => r.ExpectedDeliveryDate.DayOfWeek));
        }
    }
}
