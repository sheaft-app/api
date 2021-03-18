using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class TransferViewProfile : Profile
    {
        public TransferViewProfile()
        {
            CreateMap<Domain.Transfer, TransferInfoViewModel>();
            CreateMap<Domain.Transfer, TransferShortViewModel>();

            CreateMap<Domain.Transfer, TransferViewModel>()
                .ForMember(m => m.CreditedUser, opt => opt.MapFrom(t => t.CreditedWallet.User))
                .ForMember(m => m.DebitedUser, opt => opt.MapFrom(t => t.DebitedWallet.User));

            CreateMap<TransactionDto, TransferDto>();
            CreateMap<TransferDto, TransactionDto>();

            CreateMap<TransactionDto, TransferInfoViewModel>();
            CreateMap<TransactionDto, TransferShortViewModel>();
            CreateMap<TransactionDto, TransferViewModel>();

            CreateMap<TransferInfoViewModel, TransactionDto>();
            CreateMap<TransferShortViewModel, TransactionDto>();
            CreateMap<TransferViewModel, TransactionDto>();

            CreateMap<TransferInfoViewModel, TransferDto>();
            CreateMap<TransferShortViewModel, TransferDto>();
            CreateMap<TransferViewModel, TransferDto>();
        }
    }
}