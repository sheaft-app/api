using AutoMapper;
using Sheaft.Domain.Models;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappers
{
    public class PayoutProfile : Profile
    {
        public PayoutProfile()
        {
            CreateMap<Payout, TransactionDto>()
                .ForMember(m => m.Author, opt => opt.MapFrom(t => t.Author));

            CreateMap<Payout, PayoutDto>()
                .IncludeBase<Payout, TransactionDto>()
                .ForMember(m => m.DebitedUser, opt => opt.MapFrom(t => t.DebitedWallet.User));

            CreateMap<Payout, PayoutViewModel>()
                .ForMember(m => m.BankAccount, opt => opt.MapFrom(t => t.BankAccount))
                .ForMember(m => m.Author, opt => opt.MapFrom(t => t.Author))
                .ForMember(m => m.DebitedUser, opt => opt.MapFrom(t => t.DebitedWallet.User))
                .ForMember(m => m.Transfers, opt => opt.MapFrom(t => t.Transfers));
        }
    }
}
