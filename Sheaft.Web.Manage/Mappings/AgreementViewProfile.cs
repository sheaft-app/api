using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class AgreementViewProfile : Profile
    {
        public AgreementViewProfile()
        {
            CreateMap<Domain.Agreement, AgreementViewModel>()
                   .ForMember(c => c.Store, opt => opt.MapFrom(o => o.Store))
                   .ForMember(c => c.Delivery, opt => opt.MapFrom(o => o.Delivery))
                   .ForMember(c => c.SelectedHours, opt => opt.MapFrom(o => o.SelectedHours));

            CreateMap<AgreementViewModel, AgreementDto>();
            CreateMap<AgreementDto, AgreementViewModel>();
        }
    }
}
