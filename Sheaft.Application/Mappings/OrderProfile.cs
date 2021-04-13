using AutoMapper;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappings
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Domain.Order, OrderDto>()
                .ForMember(d => d.Donation, opt => opt.MapFrom(r => r.Donation))
                .ForMember(d => d.TotalFees, opt => opt.MapFrom(r => r.FeesPrice - r.DonationFeesPrice))
                .ForMember(d => d.PurchaseOrdersCount, opt => opt.MapFrom(r => r.PurchaseOrders.Count));
        }
    }
}
