using AutoMapper;
using Sheaft.Domain.Models;
using Sheaft.Models.Dto;

namespace Sheaft.Mappers
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            CreateMap<Transaction, BaseTransactionDto>();

            CreateMap<Transaction, TransactionDto>()
                .IncludeBase<Transaction, BaseTransactionDto>()
                .ForMember(c => c.CreditedUser, opt => opt.MapFrom(e => e.CreditedWallet.User))
                .ForMember(c => c.DebitedUser, opt => opt.MapFrom(e => e.DebitedWallet.User));

            CreateMap<WebPayinTransaction, TransactionDto>()
                .IncludeBase<Transaction, TransactionDto>();

            CreateMap<CardPayinTransaction, TransactionDto>()
                .IncludeBase<Transaction, TransactionDto>();

            CreateMap<TransferTransaction, TransactionDto>()
                .IncludeBase<Transaction, TransactionDto>();

            CreateMap<PayoutTransaction, TransactionDto>()
                .IncludeBase<Transaction, TransactionDto>();

            CreateMap<RefundPayinTransaction, TransactionDto>()
                .IncludeBase<Transaction, TransactionDto>();

            CreateMap<RefundTransferTransaction, TransactionDto>()
                .IncludeBase<Transaction, TransactionDto>();

            CreateMap<WebPayinTransaction, WebPayinTransactionDto>();
        }
    }
}
