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
                .ForMember(c => c.From, opt => opt.MapFrom(e => e.ClosedFrom))
                .ForMember(c => c.To, opt => opt.MapFrom(e => e.ClosedTo));
            CreateMap<DeliveryClosing, ClosingDto>()
                .ForMember(c => c.From, opt => opt.MapFrom(e => e.ClosedFrom))
                .ForMember(c => c.To, opt => opt.MapFrom(e => e.ClosedTo));
            CreateMap<ProductClosing, ClosingDto>()
                .ForMember(c => c.From, opt => opt.MapFrom(e => e.ClosedFrom))
                .ForMember(c => c.To, opt => opt.MapFrom(e => e.ClosedTo));
        }
    }
}
