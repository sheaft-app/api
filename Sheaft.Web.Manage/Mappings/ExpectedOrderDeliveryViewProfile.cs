using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Domain;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class ExpectedOrderDeliveryViewProfile : Profile
    {
        public ExpectedOrderDeliveryViewProfile()
        {
            CreateMap<ExpectedOrderDelivery, ExpectedOrderDeliveryViewModel>()
                 .ForMember(d => d.Day, opt => opt.MapFrom(r => r.ExpectedDeliveryDate.DayOfWeek));

            CreateMap<ExpectedOrderDeliveryDto, ExpectedOrderDeliveryViewModel>();
            CreateMap<ExpectedOrderDeliveryViewModel, ExpectedOrderDeliveryDto>();
        }
    }
}
