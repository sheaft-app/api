using AutoMapper;
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
        }
    }
}