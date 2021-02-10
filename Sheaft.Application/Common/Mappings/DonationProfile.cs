using AutoMapper;
using Sheaft.Application.Common.Models.ViewModels;

namespace Sheaft.Application.Common.Mappings
{
    public class DonationProfile : Profile
    {
        public DonationProfile()
        {
            CreateMap<Domain.Donation, DonationShortViewModel>();
            CreateMap<Domain.Donation, DonationViewModel>()
                .ForMember(m => m.Author, opt => opt.MapFrom(t => t.Author))
                .ForMember(m => m.CreditedUser, opt => opt.MapFrom(t => t.CreditedWallet.User))
                .ForMember(m => m.DebitedUser, opt => opt.MapFrom(t => t.DebitedWallet.User))
                .ForMember(m => m.Order, opt => opt.MapFrom(t => t.Order));
        }
    }
}
