using AutoMapper;
using Sheaft.Domain;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class TimeSlotViewProfile : Profile
    {
        public TimeSlotViewProfile()
        {
            CreateMap<TimeSlotHour, TimeSlotViewModel>();
        }
    }
}
