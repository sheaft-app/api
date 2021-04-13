using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class OrderViewProfile : Profile
    {
        public OrderViewProfile()
        {
            CreateMap<Domain.Order, OrderViewModel>()
                .ForMember(d => d.TotalFees, opt => opt.MapFrom(r => r.FeesPrice - r.DonationFeesPrice));

            CreateMap<Domain.Order, OrderShortViewModel>()
                .ForMember(d => d.TotalFees, opt => opt.MapFrom(r => r.FeesPrice - r.DonationFeesPrice));

            CreateMap<OrderDto, OrderViewModel>()
                .ForMember(d => d.Donate, opt => opt.MapFrom(r => r.Donation));
            CreateMap<OrderDto, OrderShortViewModel>()
                .ForMember(d => d.Donate, opt => opt.MapFrom(r => r.Donation));
            CreateMap<OrderShortViewModel, OrderDto>()
                .ForMember(d => d.Donation, opt => opt.MapFrom(r => r.Donate));
            CreateMap<OrderViewModel, OrderDto>()
                .ForMember(d => d.Donation, opt => opt.MapFrom(r => r.Donate));
        }
    }
}
