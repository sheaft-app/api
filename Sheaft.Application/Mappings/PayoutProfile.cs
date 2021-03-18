using AutoMapper;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappings
{
    public class PayoutProfile : Profile
    {
        public PayoutProfile()
        {
            CreateMap<Domain.Payout, TransactionDto>();

            CreateMap<Domain.Payout, PayoutDto>()
                .ForMember(m => m.DebitedUser, opt => opt.MapFrom(t => t.DebitedWallet.User));
        }
    }
}
