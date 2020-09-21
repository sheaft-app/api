using AutoMapper;
using Sheaft.Domain.Models;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappers
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            CreateMap<PayinTransaction, TransactionDto>()
                .ForMember(m => m.Author, opt => opt.MapFrom(t => t.Author));

            CreateMap<PayinTransaction, PayinTransactionDto>()
                .IncludeBase<PayinTransaction, TransactionDto>()
                .ForMember(m => m.CreditedUser, opt => opt.MapFrom(t => t.CreditedWallet.User))
                .ForMember(m => m.Order, opt => opt.MapFrom(t => t.Order));

            CreateMap<CardPayinTransaction,TransactionDto>()
                .IncludeBase<PayinTransaction, TransactionDto>();

            CreateMap<WebPayinTransaction, WebPayinTransactionDto>()
                .IncludeBase<PayinTransaction, TransactionDto>();

            CreateMap<TransferTransaction, TransactionDto>()
                .ForMember(m => m.Author, opt => opt.MapFrom(t => t.Author));

            CreateMap<TransferTransaction, TransferTransactionDto>()
                .IncludeBase<TransferTransaction, TransactionDto>()
                .ForMember(m => m.CreditedUser, opt => opt.MapFrom(t => t.CreditedWallet.User))
                .ForMember(m => m.DebitedUser, opt => opt.MapFrom(t => t.DebitedWallet.User))
                .ForMember(m => m.PurchaseOrder, opt => opt.MapFrom(t => t.PurchaseOrder));

            CreateMap<PayoutTransaction, TransactionDto>()
                .ForMember(m => m.Author, opt => opt.MapFrom(t => t.Author));

            CreateMap<PayoutTransaction, PayoutTransactionDto>()
                .IncludeBase<PayoutTransaction, TransactionDto>()
                .ForMember(m => m.DebitedUser, opt => opt.MapFrom(t => t.DebitedWallet.User));

            CreateMap<RefundTransaction, TransactionDto>()
                .ForMember(m => m.Author, opt => opt.MapFrom(t => t.Author));

            CreateMap<RefundTransaction, RefundTransactionDto>()
                .IncludeBase<RefundTransaction, TransactionDto>();

            CreateMap<RefundPayinTransaction, RefundTransactionDto>()
                .IncludeBase<RefundTransaction, RefundTransactionDto>()
                .ForMember(m => m.RefundedTransaction, opt => opt.MapFrom(t => t.PayinTransaction));

            CreateMap<RefundPayinTransaction, RefundPayinTransactionDto>()
                .IncludeBase<RefundPayinTransaction, RefundTransactionDto>()
                .ForMember(m => m.DebitedUser, opt => opt.MapFrom(t => t.DebitedWallet.User));

            CreateMap<RefundTransferTransaction, RefundTransactionDto>()
                .IncludeBase<RefundTransaction, RefundTransactionDto>()
                .ForMember(m => m.RefundedTransaction, opt => opt.MapFrom(t => t.TransferTransaction));

            CreateMap<RefundTransferTransaction, RefundTransferTransactionDto>()
                .IncludeBase<RefundTransferTransaction, RefundTransactionDto>()
                .ForMember(m => m.CreditedUser, opt => opt.MapFrom(t => t.CreditedWallet.User))
                .ForMember(m => m.DebitedUser, opt => opt.MapFrom(t => t.DebitedWallet.User));
        }
    }
}
