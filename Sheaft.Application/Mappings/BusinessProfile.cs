using System;
using System.Linq;
using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Mappings
{
    public class BusinessProfile : Profile
    {
        public BusinessProfile()
        {
            CreateMap<Domain.Business, UserDto>();
            
            CreateMap<Domain.Business, ProducerDto>()
                .IncludeBase<Domain.Business, UserDto>()
                .ForMember(c => c.Closings,
                    opt => opt.MapFrom(e => e.Closings.Where(c => c.ClosedTo > DateTimeOffset.UtcNow)));
            
            CreateMap<Domain.Business, StoreDto>()
                .IncludeBase<Domain.Business, UserDto>()
                .ForMember(c => c.Closings,
                    opt => opt.MapFrom(e => e.Closings.Where(c => c.ClosedTo > DateTimeOffset.UtcNow)));
        }
    }
}
