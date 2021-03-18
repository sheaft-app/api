using AutoMapper;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappings
{
    public class TransferProfile : Profile
    {
        public TransferProfile()
        {
            CreateMap<Domain.Transfer, TransactionDto>();

            CreateMap<Domain.Transfer, TransferDto>()
                .ForMember(m => m.CreditedUser, opt => opt.MapFrom(t => t.CreditedWallet.User))
                .ForMember(m => m.DebitedUser, opt => opt.MapFrom(t => t.DebitedWallet.User));

            CreateMap<TransactionDto, TransferDto>();
            CreateMap<TransferDto, TransactionDto>();
        }
    }
}