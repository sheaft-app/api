using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class DonationViewProfile : Profile
    {
        public DonationViewProfile()
        {
            CreateMap<Domain.Donation, DonationViewModel>()
                .ForMember(m => m.CreditedUser, opt => opt.MapFrom(t => t.CreditedWallet.User));

            CreateMap<Domain.Donation, DonationShortViewModel>();

            CreateMap<DonationDto, DonationViewModel>();
            CreateMap<DonationDto, DonationShortViewModel>();
            CreateMap<DonationShortViewModel, DonationDto>();
            CreateMap<DonationViewModel, DonationDto>();
            CreateMap<DonationViewModel, DonationShortViewModel>();
            CreateMap<DonationShortViewModel, DonationViewModel>();
        }
    }
}
