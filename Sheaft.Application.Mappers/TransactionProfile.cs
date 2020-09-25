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

            CreateMap<Payin, PayinDto>()
                .IncludeBase<Payin, TransactionDto>()
                .ForMember(m => m.CreditedUser, opt => opt.MapFrom(t => t.CreditedWallet.User))
                .ForMember(m => m.Order, opt => opt.MapFrom(t => t.Order));

            CreateMap<CardPayin,TransactionDto>()
                .IncludeBase<Payin, TransactionDto>();

            CreateMap<WebPayin, WebPayinDto>()
                .IncludeBase<Payin, TransactionDto>();

            CreateMap<Transfer, TransactionDto>()
                .ForMember(m => m.Author, opt => opt.MapFrom(t => t.Author));

            CreateMap<Transfer, TransferDto>()
                .IncludeBase<Transfer, TransactionDto>()
                .ForMember(m => m.CreditedUser, opt => opt.MapFrom(t => t.CreditedWallet.User))
                .ForMember(m => m.DebitedUser, opt => opt.MapFrom(t => t.DebitedWallet.User))
                .ForMember(m => m.PurchaseOrder, opt => opt.MapFrom(t => t.PurchaseOrder));

            CreateMap<Payout, TransactionDto>()
                .ForMember(m => m.Author, opt => opt.MapFrom(t => t.Author));

            CreateMap<Payout, PayoutDto>()
                .IncludeBase<Payout, TransactionDto>()
                .ForMember(m => m.DebitedUser, opt => opt.MapFrom(t => t.DebitedWallet.User));

            CreateMap<Refund, TransactionDto>()
                .ForMember(m => m.Author, opt => opt.MapFrom(t => t.Author));

            CreateMap<Refund, RefundDto>()
                .IncludeBase<Refund, TransactionDto>();

            CreateMap<PayinRefund, RefundDto>()
                .IncludeBase<Refund, RefundDto>();

            CreateMap<PayinRefund, PayinRefundDto>()
                .IncludeBase<PayinRefund, RefundDto>()
                .ForMember(m => m.Payin, opt => opt.MapFrom(t => t.Payin))
                .ForMember(m => m.DebitedUser, opt => opt.MapFrom(t => t.DebitedWallet.User));

            CreateMap<TransferRefund, RefundDto>()
                .IncludeBase<Refund, RefundDto>();

            CreateMap<TransferRefund, TransferRefundDto>()
                .IncludeBase<TransferRefund, RefundDto>()
                .ForMember(m => m.CreditedUser, opt => opt.MapFrom(t => t.CreditedWallet.User))
                .ForMember(m => m.DebitedUser, opt => opt.MapFrom(t => t.DebitedWallet.User))
                .ForMember(m => m.Transfer, opt => opt.MapFrom(t => t.Transfer));
        }
    }
}
