using AutoMapper;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Application.Common.Models.ViewModels;

namespace Sheaft.Mappings
{
    public class DonationProfile : Profile
    {
        public DonationProfile()
        {
            CreateMap<Domain.Donation, TransactionDto>();

            CreateMap<Domain.Donation, DonationDto>()
                .IncludeBase<Domain.Donation, TransactionDto>()
                .ForMember(m => m.CreditedUser, opt => opt.MapFrom(t => t.CreditedWallet.User));

            CreateMap<Domain.Donation, DonationViewModel>()
                .ForMember(m => m.CreditedUser, opt => opt.MapFrom(t => t.CreditedWallet.User));
        }
    }
}
