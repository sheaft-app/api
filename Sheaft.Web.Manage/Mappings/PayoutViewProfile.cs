using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class PayoutViewProfile : Profile
    {
        public PayoutViewProfile()
        {
            CreateMap<Domain.Payout, PayoutShortViewModel>();
            
            CreateMap<Domain.Payout, PayoutViewModel>()
                .ForMember(m => m.DebitedUser, opt => opt.MapFrom(t => t.DebitedWallet.User));

            CreateMap<PayoutDto, PayoutViewModel>();
            CreateMap<PayoutDto, PayoutShortViewModel>();
            CreateMap<PayoutViewModel, PayoutDto>();
            CreateMap<PayoutShortViewModel, PayoutDto>();
            CreateMap<PayoutViewModel, PayoutShortViewModel>();
            CreateMap<PayoutShortViewModel, PayoutViewModel>();
        }
    }
}
