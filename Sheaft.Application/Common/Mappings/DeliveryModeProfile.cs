using AutoMapper;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Application.Common.Models.Inputs;
using Sheaft.Application.Common.Models.ViewModels;
using Sheaft.Application.DeliveryMode.Commands;

namespace Sheaft.Application.Common.Mappings
{
    public class DeliveryModeProfile : Profile
    {
        public DeliveryModeProfile()
        {
            CreateMap<Domain.DeliveryMode, DeliveryModeDto>()
                .ForMember(d => d.Address, opt => opt.MapFrom(r => r.Address))
                .ForMember(d => d.Producer, opt => opt.MapFrom(r => r.Producer))
                .ForMember(d => d.OpeningHours, opt => opt.MapFrom(r => r.OpeningHours));

            CreateMap<Domain.DeliveryMode, DeliveryModeViewModel>()
                .ForMember(d => d.Address, opt => opt.MapFrom(r => r.Address))
                .ForMember(d => d.Producer, opt => opt.MapFrom(r => r.Producer))
                .ForMember(d => d.OpeningHours, opt => opt.MapFrom(r => r.OpeningHours));

            CreateMap<Domain.DeliveryMode, AgreementDeliveryModeDto>()
                .ForMember(d => d.Address, opt => opt.MapFrom(r => r.Address))
                .ForMember(d => d.Producer, opt => opt.MapFrom(r => r.Producer));

            CreateMap<CreateDeliveryModeInput, CreateDeliveryModeCommand>();
            CreateMap<UpdateDeliveryModeInput, UpdateDeliveryModeCommand>()
                .ForMember(c => c.DeliveryModeId, opt => opt.MapFrom(r => r.Id));;
            CreateMap<IdInput, DeleteDeliveryModeCommand>()
                    .ForMember(c => c.DeliveryModeId, opt => opt.MapFrom(r => r.Id));
            CreateMap<SetDeliveryModesAvailabilityInput, SetDeliveryModesAvailabilityCommand>()
                .ForMember(c => c.DeliveryModeIds, opt => opt.MapFrom(r => r.Ids));
        }
    }
}
