using AutoMapper;
using Sheaft.Domain.Models;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappers
{
    public class DonationProfile : Profile
    {
        public DonationProfile()
        {
            CreateMap<Donation, DonationShortViewModel>();
            CreateMap<Donation, DonationViewModel>()
                .ForMember(m => m.Author, opt => opt.MapFrom(t => t.Author))
                .ForMember(m => m.CreditedUser, opt => opt.MapFrom(t => t.CreditedWallet.User))
                .ForMember(m => m.DebitedUser, opt => opt.MapFrom(t => t.DebitedWallet.User))
                .ForMember(m => m.Order, opt => opt.MapFrom(t => t.Order));
        }
    }
}
