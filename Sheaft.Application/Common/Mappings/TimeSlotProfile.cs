using AutoMapper;
using Sheaft.Domain.Models;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappers
{
    public class TimeSlotProfile : Profile
    {
        public TimeSlotProfile()
        {
            CreateMap<TimeSlotHour, TimeSlotDto>();
            CreateMap<TimeSlotHour, TimeSlotViewModel>();
        }
    }
}
