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
            CreateMap<WebPayin, TransactionDto>();
            CreateMap<PreAuthorizedPayin, TransactionDto>();
            CreateMap<PreAuthorizedPayin, PreAuthorizedPayinDto>();
            
            CreateMap<Payin, PayinDto>()
                .ForMember(m => m.CreditedUser, opt => opt.MapFrom(t => t.CreditedWallet.User));
            CreateMap<WebPayin, PayinDto>()
                .ForMember(m => m.CreditedUser, opt => opt.MapFrom(t => t.CreditedWallet.User));
            CreateMap<PreAuthorizedPayin, PayinDto>()
                .ForMember(m => m.CreditedUser, opt => opt.MapFrom(t => t.CreditedWallet.User));
            
        }
    }
}
