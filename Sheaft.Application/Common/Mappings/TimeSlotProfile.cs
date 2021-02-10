using AutoMapper;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Application.Common.Models.ViewModels;
using Sheaft.Domain;

namespace Sheaft.Application.Common.Mappings
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
