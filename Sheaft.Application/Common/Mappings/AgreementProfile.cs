using AutoMapper;
using Sheaft.Application.Agreement.Commands;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Application.Common.Models.Inputs;
using Sheaft.Application.Common.Models.ViewModels;

namespace Sheaft.Application.Common.Mappings
{
    public class AgreementProfile : Profile
    {
        public AgreementProfile()
        {
            CreateMap<Domain.Agreement, AgreementDto>()
                   .ForMember(c => c.Store, opt => opt.MapFrom(o => o.Store))
                   .ForMember(c => c.Delivery, opt => opt.MapFrom(o => o.Delivery))
                   .ForMember(c => c.SelectedHours, opt => opt.MapFrom(o => o.SelectedHours));

            CreateMap<Domain.Agreement, AgreementViewModel>()
                   .ForMember(c => c.Store, opt => opt.MapFrom(o => o.Store))
                   .ForMember(c => c.Delivery, opt => opt.MapFrom(o => o.Delivery))
                   .ForMember(c => c.SelectedHours, opt => opt.MapFrom(o => o.SelectedHours));

            CreateMap<CreateAgreementInput, CreateAgreementCommand>();
            CreateMap<IdTimeSlotGroupInput, AcceptAgreementCommand>();
            CreateMap<IdsWithReasonInput, CancelAgreementsCommand>();
            CreateMap<IdsWithReasonInput, RefuseAgreementsCommand>();
        }
    }
}
