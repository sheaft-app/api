using AutoMapper;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappings
{
    public class WithholdingProfile : Profile
    {
        public WithholdingProfile()
        {
            CreateMap<Domain.Withholding, TransactionDto>();

            CreateMap<Domain.Withholding, WithholdingDto>()
                .ForMember(m => m.CreditedUser, opt => opt.MapFrom(t => t.CreditedWallet.User));
        }
    }
}
