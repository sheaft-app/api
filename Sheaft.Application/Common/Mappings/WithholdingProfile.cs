using AutoMapper;
using Sheaft.Domain.Models;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappers
{
    public class WithholdingProfile : Profile
    {
        public WithholdingProfile()
        {
            CreateMap<Withholding, WithholdingShortViewModel>();
            CreateMap<Withholding, WithholdingViewModel>()
                .ForMember(m => m.Author, opt => opt.MapFrom(t => t.Author))
                .ForMember(m => m.DebitedUser, opt => opt.MapFrom(t => t.DebitedWallet.User))
                .ForMember(m => m.CreditedUser, opt => opt.MapFrom(t => t.CreditedWallet.User));
        }
    }
}
