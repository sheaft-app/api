using AutoMapper;
using Sheaft.Domain.Models;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappers
{
    public class TransferProfile : Profile
    {
        public TransferProfile()
        {
            CreateMap<Transfer, TransactionDto>()
                .ForMember(m => m.Author, opt => opt.MapFrom(t => t.Author));

            CreateMap<Transfer, TransferDto>()
                .IncludeBase<Transfer, TransactionDto>()
                .ForMember(m => m.CreditedUser, opt => opt.MapFrom(t => t.CreditedWallet.User))
                .ForMember(m => m.DebitedUser, opt => opt.MapFrom(t => t.DebitedWallet.User))
                .ForMember(m => m.PurchaseOrder, opt => opt.MapFrom(t => t.PurchaseOrder));

            CreateMap<Transfer, TransferShortViewModel>()
                .ForMember(m => m.PurchaseOrder, opt => opt.MapFrom(t => t.PurchaseOrder));

            CreateMap<Transfer, TransferViewModel>()
                .ForMember(m => m.Author, opt => opt.MapFrom(t => t.Author))
                .ForMember(m => m.CreditedUser, opt => opt.MapFrom(t => t.CreditedWallet.User))
                .ForMember(m => m.DebitedUser, opt => opt.MapFrom(t => t.DebitedWallet.User))
                .ForMember(m => m.PurchaseOrder, opt => opt.MapFrom(t => t.PurchaseOrder));
        }
    }
}
