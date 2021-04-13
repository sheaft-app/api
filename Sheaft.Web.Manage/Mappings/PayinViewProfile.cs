using AutoMapper;
using Sheaft.Application.Models;
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

            CreateMap<PayinDto, PayinViewModel>();
            CreateMap<PayinDto, PayinShortViewModel>();
            CreateMap<PayinViewModel, PayinDto>();
            CreateMap<PayinShortViewModel, PayinDto>();
            
            CreateMap<PayinDto, PayinViewModel>();
            CreateMap<PayinDto, PayinShortViewModel>();
            CreateMap<PayinViewModel, PayinDto>();
            CreateMap<PayinShortViewModel, PayinDto>();
            
            CreateMap<TransactionDto, PayinViewModel>();
            CreateMap<TransactionDto, PayinShortViewModel>();
            CreateMap<PayinShortViewModel, TransactionDto>();
            CreateMap<PayinViewModel, TransactionDto>();
            
            CreateMap<PayinViewModel, PayinShortViewModel>();
            CreateMap<PayinShortViewModel, PayinViewModel>();
        }
    }
}
