using AutoMapper;
using Sheaft.Domain.Models;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappers
{
    public class PayinRefundProfile : Profile
    {
        public PayinRefundProfile()
        {
            CreateMap<WebPayinRefund, PayinRefundDto>()
                .ForMember(m => m.Author, opt => opt.MapFrom(t => t.Author))
                .ForMember(m => m.Payin, opt => opt.MapFrom(t => t.Payin))
                .ForMember(m => m.DebitedUser, opt => opt.MapFrom(t => t.DebitedWallet.User));
        }
    }
}
