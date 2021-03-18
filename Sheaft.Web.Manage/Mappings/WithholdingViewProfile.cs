using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class WithholdingViewProfile : Profile
    {
        public WithholdingViewProfile()
        {
            CreateMap<Domain.Withholding, WithholdingShortViewModel>();
            CreateMap<Domain.Withholding, WithholdingViewModel>()
                .ForMember(m => m.DebitedUser, opt => opt.MapFrom(t => t.DebitedWallet.User))
                .ForMember(m => m.CreditedUser, opt => opt.MapFrom(t => t.CreditedWallet.User));

            CreateMap<TransactionDto, WithholdingShortViewModel>();
            CreateMap<TransactionDto, WithholdingViewModel>();
            CreateMap<WithholdingShortViewModel, TransactionDto>();
            CreateMap<WithholdingViewModel, TransactionDto>();
            
            CreateMap<WithholdingDto, WithholdingShortViewModel>();
            CreateMap<WithholdingDto, WithholdingViewModel>();
            CreateMap<WithholdingShortViewModel, WithholdingDto>();
            CreateMap<WithholdingViewModel, WithholdingDto>();
        }
    }
}
