using System;
using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Mappings
{
    public class ClosingProfile : Profile
    {
        public ClosingProfile()
        {
            CreateMap<BusinessClosing, ClosingDto>()
                .ForMember(c => c.From, opt => opt.MapFrom(e => new DateTimeOffset(new DateTime(e.ClosedFrom.Year, e.ClosedFrom.Month, e.ClosedFrom.Day, 0, 0, 0, DateTimeKind.Utc))))
                .ForMember(c => c.To, opt => opt.MapFrom(e => new DateTimeOffset(new DateTime(e.ClosedTo.Year, e.ClosedTo.Month, e.ClosedTo.Day, 0, 0, 0, DateTimeKind.Utc))));
            CreateMap<DeliveryClosing, ClosingDto>()
                .ForMember(c => c.From, opt => opt.MapFrom(e => new DateTimeOffset(new DateTime(e.ClosedFrom.Year, e.ClosedFrom.Month, e.ClosedFrom.Day, 0, 0, 0, DateTimeKind.Utc))))
                .ForMember(c => c.To, opt => opt.MapFrom(e => new DateTimeOffset(new DateTime(e.ClosedTo.Year, e.ClosedTo.Month, e.ClosedTo.Day, 0, 0, 0, DateTimeKind.Utc))));
        }
    }
}