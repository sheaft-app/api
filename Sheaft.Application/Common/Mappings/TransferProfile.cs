using AutoMapper;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Application.Common.Models.ViewModels;

namespace Sheaft.Application.Common.Mappings
{
    public class TransferProfile : Profile
    {
        public TransferProfile()
        {
            CreateMap<Domain.Transfer, TransactionDto>()
                .ForMember(m => m.Author, opt => opt.MapFrom(t => t.Author));

            CreateMap<Domain.Transfer, TransferDto>()
                .IncludeBase<Domain.Transfer, TransactionDto>()
                .ForMember(m => m.CreditedUser, opt => opt.MapFrom(t => t.CreditedWallet.User))
                .ForMember(m => m.DebitedUser, opt => opt.MapFrom(t => t.DebitedWallet.User))
                .ForMember(m => m.PurchaseOrder, opt => opt.MapFrom(t => t.PurchaseOrder));

            CreateMap<Domain.Transfer, TransferInfoViewModel>();

            CreateMap<Domain.Transfer, TransferShortViewModel>()
                .ForMember(m => m.PurchaseOrder, opt => opt.MapFrom(t => t.PurchaseOrder));

            CreateMap<Domain.Transfer, TransferViewModel>()
                .ForMember(m => m.Author, opt => opt.MapFrom(t => t.Author))
                .ForMember(m => m.CreditedUser, opt => opt.MapFrom(t => t.CreditedWallet.User))
                .ForMember(m => m.DebitedUser, opt => opt.MapFrom(t => t.DebitedWallet.User))
                .ForMember(m => m.PurchaseOrder, opt => opt.MapFrom(t => t.PurchaseOrder));
        }
    }
}
