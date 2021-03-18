using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Domain;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class TimeSlotViewProfile : Profile
    {
        public TimeSlotViewProfile()
        {
            CreateMap<TimeSlotHour, TimeSlotViewModel>();
            CreateMap<TimeSlotDto, TimeSlotViewModel>();
            CreateMap<TimeSlotViewModel, TimeSlotDto>();
        }
    }
}
