using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Mappings
{
    public class PayinProfile : Profile
    {
        public PayinProfile()
        {
            CreateMap<Payin, TransactionDto>();
            CreateMap<CardPayin, TransactionDto>();
            CreateMap<WebPayin, TransactionDto>();
            CreateMap<WebPayin, WebPayinDto>();

            CreateMap<Payin, PayinDto>()
                .ForMember(m => m.CreditedUser, opt => opt.MapFrom(t => t.CreditedWallet.User));
        }
    }
}
