using AutoMapper;
using Sheaft.Application.Agreement.Commands;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Application.Common.Models.Inputs;
using Sheaft.Application.Common.Models.ViewModels;

namespace Sheaft.Mappings
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
            CreateMap<IdTimeSlotGroupInput, AcceptAgreementCommand>()
                .ForMember(c => c.AgreementId, opt => opt.MapFrom(r => r.Id));
            CreateMap<IdsWithReasonInput, CancelAgreementsCommand>()
                .ForMember(c => c.AgreementIds, opt => opt.MapFrom(r => r.Ids));
            CreateMap<IdsWithReasonInput, RefuseAgreementsCommand>()
                .ForMember(c => c.AgreementIds, opt => opt.MapFrom(r => r.Ids));
        }
    }
}
