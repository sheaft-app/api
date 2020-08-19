using AutoMapper;
using Sheaft.Domain.Models;
using Sheaft.Models.Dto;
using Sheaft.Models.ViewModels;

namespace Sheaft.Mappers
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
