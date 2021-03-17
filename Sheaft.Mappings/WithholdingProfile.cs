using AutoMapper;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Application.Common.Models.ViewModels;

namespace Sheaft.Mappings
{
    public class WithholdingProfile : Profile
    {
        public WithholdingProfile()
        {
            CreateMap<Domain.Withholding, TransactionDto>();

            CreateMap<Domain.Withholding, WithholdingDto>()
                .IncludeBase<Domain.Withholding, TransactionDto>()
                .ForMember(m => m.CreditedUser, opt => opt.MapFrom(t => t.CreditedWallet.User));

            CreateMap<Domain.Withholding, WithholdingShortViewModel>();
            CreateMap<Domain.Withholding, WithholdingViewModel>()
                .ForMember(m => m.Author, opt => opt.MapFrom(t => t.Author))
                .ForMember(m => m.DebitedUser, opt => opt.MapFrom(t => t.DebitedWallet.User))
                .ForMember(m => m.CreditedUser, opt => opt.MapFrom(t => t.CreditedWallet.User));
        }
    }
}
