using AutoMapper;
using Sheaft.Domain.Models;
using Sheaft.Models.Dto;

namespace Sheaft.Mappers
{
    public class TimeSlotProfile : Profile
    {
        public TimeSlotProfile()
        {
            CreateMap<TimeSlotHour, TimeSlotDto>();
        }
    }
}
