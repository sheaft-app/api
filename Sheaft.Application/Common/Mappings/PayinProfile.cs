using AutoMapper;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Application.Common.Models.ViewModels;
using Sheaft.Domain;

namespace Sheaft.Application.Common.Mappings
{
    public class PayinProfile : Profile
    {
        public PayinProfile()
        {
            CreateMap<Domain.Payin, TransactionDto>()
                .ForMember(m => m.Author, opt => opt.MapFrom(t => t.Author));

            CreateMap<Domain.Payin, PayinDto>()
                .IncludeBase<Domain.Payin, TransactionDto>()
                .ForMember(m => m.CreditedUser, opt => opt.MapFrom(t => t.CreditedWallet.User))
                .ForMember(m => m.Order, opt => opt.MapFrom(t => t.Order));

            CreateMap<CardPayin,TransactionDto>()
                .IncludeBase<Domain.Payin, TransactionDto>();

            CreateMap<WebPayin, WebPayinDto>()
                .IncludeBase<Domain.Payin, TransactionDto>();

            CreateMap<Domain.Payin, PayinShortViewModel>();

            CreateMap<Domain.Payin, PayinViewModel>()
                .ForMember(m => m.Author, opt => opt.MapFrom(t => t.Author))
                .ForMember(m => m.CreditedUser, opt => opt.MapFrom(t => t.CreditedWallet.User))
                .ForMember(m => m.Order, opt => opt.MapFrom(t => t.Order));
        }
    }
}
