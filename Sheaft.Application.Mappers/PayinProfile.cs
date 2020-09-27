using AutoMapper;
using Sheaft.Domain.Models;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappers
{
    public class PayinProfile : Profile
    {
        public PayinProfile()
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
        }
    }
}
