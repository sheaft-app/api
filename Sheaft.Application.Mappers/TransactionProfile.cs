using AutoMapper;
using Sheaft.Domain.Models;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappers
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            CreateMap<Payin, TransactionDto>()
                .ForMember(m => m.Author, opt => opt.MapFrom(t => t.Author));

            CreateMap<Payin, PayinTransactionDto>()
                .IncludeBase<Payin, TransactionDto>()
                .ForMember(m => m.CreditedUser, opt => opt.MapFrom(t => t.CreditedWallet.User))
                .ForMember(m => m.Order, opt => opt.MapFrom(t => t.Order));

            CreateMap<CardPayin,TransactionDto>()
                .IncludeBase<Payin, TransactionDto>();

            CreateMap<WebPayin, WebPayinTransactionDto>()
                .IncludeBase<Payin, TransactionDto>();

            CreateMap<Transfer, TransactionDto>()
                .ForMember(m => m.Author, opt => opt.MapFrom(t => t.Author));

            CreateMap<Transfer, TransferTransactionDto>()
                .IncludeBase<Transfer, TransactionDto>()
                .ForMember(m => m.CreditedUser, opt => opt.MapFrom(t => t.CreditedWallet.User))
                .ForMember(m => m.DebitedUser, opt => opt.MapFrom(t => t.DebitedWallet.User))
                .ForMember(m => m.PurchaseOrder, opt => opt.MapFrom(t => t.PurchaseOrder));

            CreateMap<Payout, TransactionDto>()
                .ForMember(m => m.Author, opt => opt.MapFrom(t => t.Author));

            CreateMap<Payout, PayoutTransactionDto>()
                .IncludeBase<Payout, TransactionDto>()
                .ForMember(m => m.DebitedUser, opt => opt.MapFrom(t => t.DebitedWallet.User));

            CreateMap<Refund, TransactionDto>()
                .ForMember(m => m.Author, opt => opt.MapFrom(t => t.Author));

            CreateMap<Refund, RefundTransactionDto>()
                .IncludeBase<Refund, TransactionDto>();

            CreateMap<PayinRefund, RefundTransactionDto>()
                .IncludeBase<Refund, RefundTransactionDto>()
                .ForMember(m => m.RefundedTransaction, opt => opt.MapFrom(t => t.Payin));

            CreateMap<PayinRefund, RefundPayinTransactionDto>()
                .IncludeBase<PayinRefund, RefundTransactionDto>()
                .ForMember(m => m.DebitedUser, opt => opt.MapFrom(t => t.DebitedWallet.User));

            CreateMap<TransferRefund, RefundTransactionDto>()
                .IncludeBase<Refund, RefundTransactionDto>()
                .ForMember(m => m.RefundedTransaction, opt => opt.MapFrom(t => t.Transfer));

            CreateMap<TransferRefund, RefundTransferTransactionDto>()
                .IncludeBase<TransferRefund, RefundTransactionDto>()
                .ForMember(m => m.CreditedUser, opt => opt.MapFrom(t => t.CreditedWallet.User))
                .ForMember(m => m.DebitedUser, opt => opt.MapFrom(t => t.DebitedWallet.User));
        }
    }
}
