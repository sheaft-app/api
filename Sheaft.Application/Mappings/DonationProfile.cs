using AutoMapper;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappings
{
    public class DonationProfile : Profile
    {
        public DonationProfile()
        {
            CreateMap<Domain.Donation, TransactionDto>();

            CreateMap<Domain.Donation, DonationDto>()
                .ForMember(m => m.CreditedUser, opt => opt.MapFrom(t => t.CreditedWallet.User));
        }
    }
}
