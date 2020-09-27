using AutoMapper;
using Sheaft.Domain.Models;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappers
{
    public class TransferRefundProfile : Profile
    {
        public TransferRefundProfile()
        {
            CreateMap<TransferRefund, TransferRefundDto>()
                .ForMember(m => m.Author, opt => opt.MapFrom(t => t.Author))
                .ForMember(m => m.CreditedUser, opt => opt.MapFrom(t => t.CreditedWallet.User))
                .ForMember(m => m.DebitedUser, opt => opt.MapFrom(t => t.DebitedWallet.User))
                .ForMember(m => m.Transfer, opt => opt.MapFrom(t => t.Transfer));
        }
    }
}
