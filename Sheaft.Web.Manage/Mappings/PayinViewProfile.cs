using AutoMapper;
using Sheaft.Domain;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class PayinViewProfile : Profile
    {
        public PayinViewProfile()
        {
            CreateMap<Payin, PayinShortViewModel>();
            CreateMap<PreAuthorizedPayin, PayinShortViewModel>();
            CreateMap<WebPayin, PayinShortViewModel>();
            
            CreateMap<Payin, PayinViewModel>()
                .ForMember(m => m.CreditedUser, opt => opt.MapFrom(t => t.CreditedWallet.User));
            
            CreateMap<PreAuthorizedPayin, PayinViewModel>()
                .ForMember(m => m.CreditedUser, opt => opt.MapFrom(t => t.CreditedWallet.User));

            CreateMap<WebPayin, PayinViewModel>()
                .ForMember(m => m.CreditedUser, opt => opt.MapFrom(t => t.CreditedWallet.User));
            
            CreateMap<PayinViewModel, PayinShortViewModel>();
            CreateMap<PayinShortViewModel, PayinViewModel>();
        }
    }
}
