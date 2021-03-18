using AutoMapper;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappings
{
    public class PayinRefundProfile : Profile
    {
        public PayinRefundProfile()
        {
            CreateMap<Domain.PayinRefund, PayinRefundDto>()
                .ForMember(m => m.DebitedUser, opt => opt.MapFrom(t => t.DebitedWallet.User));
        }
    }
}
