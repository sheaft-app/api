using AutoMapper;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Application.Common.Models.ViewModels;
using Sheaft.Domain;

namespace Sheaft.Mappings
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
