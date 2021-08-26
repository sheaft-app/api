using AutoMapper;
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

            CreateMap<PayoutViewModel, PayoutShortViewModel>();
            CreateMap<PayoutShortViewModel, PayoutViewModel>();
        }
    }
}
