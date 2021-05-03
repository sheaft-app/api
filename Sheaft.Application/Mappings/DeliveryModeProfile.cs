using System;
using System.Linq;
using AutoMapper;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappings
{
    public class DeliveryModeProfile : Profile
    {
        public DeliveryModeProfile()
        {
            CreateMap<Domain.DeliveryMode, DeliveryModeDto>()
                .ForMember(c => c.Closings,
                    opt => opt.MapFrom(e => e.Closings.Where(c => c.ClosedTo > DateTimeOffset.UtcNow)));
        }
    }
}
