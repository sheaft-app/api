using AutoMapper;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappings
{
    public class AgreementProfile : Profile
    {
        public AgreementProfile()
        {
            CreateMap<Domain.Agreement, AgreementDto>()
                   .ForMember(c => c.Store, opt => opt.MapFrom(o => o.Store))
                   .ForMember(c => c.Delivery, opt => opt.MapFrom(o => o.Delivery))
                   .ForMember(c => c.SelectedHours, opt => opt.MapFrom(o => o.SelectedHours));
        }
    }
}
