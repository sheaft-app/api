using AutoMapper;
using Sheaft.Application.Common.Models.Dto;

namespace Sheaft.Application.Common.Mappings
{
    public class PayinRefundProfile : Profile
    {
        public PayinRefundProfile()
        {
            CreateMap<Domain.PayinRefund, PayinRefundDto>()
                .ForMember(m => m.Author, opt => opt.MapFrom(t => t.Author))
                .ForMember(m => m.Payin, opt => opt.MapFrom(t => t.Payin))
                .ForMember(m => m.DebitedUser, opt => opt.MapFrom(t => t.DebitedWallet.User));
        }
    }
}
