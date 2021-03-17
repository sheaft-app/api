using AutoMapper;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Application.Common.Models.ViewModels;

namespace Sheaft.Mappings
{
    public class PayoutProfile : Profile
    {
        public PayoutProfile()
        {
            CreateMap<Domain.Payout, TransactionDto>()
                .ForMember(m => m.Author, opt => opt.MapFrom(t => t.Author));

            CreateMap<Domain.Payout, PayoutDto>()
                .IncludeBase<Domain.Payout, TransactionDto>()
                .ForMember(m => m.DebitedUser, opt => opt.MapFrom(t => t.DebitedWallet.User));

            CreateMap<Domain.Payout, PayoutShortViewModel>();
            CreateMap<Domain.Payout, PayoutViewModel>()
                .ForMember(m => m.BankAccount, opt => opt.MapFrom(t => t.BankAccount))
                .ForMember(m => m.Author, opt => opt.MapFrom(t => t.Author))
                .ForMember(m => m.DebitedUser, opt => opt.MapFrom(t => t.DebitedWallet.User))
                .ForMember(m => m.Transfers, opt => opt.MapFrom(t => t.Transfers))
                .ForMember(m => m.Withholdings, opt => opt.MapFrom(t => t.Withholdings));
        }
    }
}
