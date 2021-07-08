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
                .ForMember(d => d.Donate, opt => opt.MapFrom(r => r.Donation))
                .ForMember(d => d.TotalFees, opt => opt.MapFrom(r => r.FeesPrice - r.DonationFeesPrice));

            CreateMap<Domain.Order, ShortOrderViewModel>()
                .ForMember(d => d.Donate, opt => opt.MapFrom(r => r.Donation))
                .ForMember(d => d.TotalFees, opt => opt.MapFrom(r => r.FeesPrice - r.DonationFeesPrice));
        }
    }
}
