using AutoMapper;
using Sheaft.Domain.Models;
using Sheaft.Models.Dto;

namespace Sheaft.Mappers
{
    public class ExpectedDeliveryProfile : Profile
    {
        public ExpectedDeliveryProfile()
        {
            CreateMap<ExpectedDelivery, ExpectedDeliveryDto>()
                 .ForMember(d => d.Address, opt => opt.MapFrom(r => r.Address))
                 .ForMember(d => d.Day, opt => opt.MapFrom(r => r.ExpectedDeliveryDate.DayOfWeek));
        }
    }
}
