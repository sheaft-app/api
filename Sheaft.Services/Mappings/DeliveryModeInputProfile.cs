using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Services.DeliveryMode.Commands;

namespace Sheaft.Services.Mappings
{
    public class DeliveryModeInputProfile : Profile
    {
        public DeliveryModeInputProfile()
        {
            CreateMap<CreateDeliveryModeDto, CreateDeliveryModeCommand>();
            CreateMap<UpdateDeliveryModeDto, UpdateDeliveryModeCommand>()
                .ForMember(c => c.DeliveryModeId, opt => opt.MapFrom(r => r.Id));;
            CreateMap<ResourceIdDto, DeleteDeliveryModeCommand>()
                    .ForMember(c => c.DeliveryModeId, opt => opt.MapFrom(r => r.Id));
            CreateMap<SetResourceIdsAvailabilityDto, SetDeliveryModesAvailabilityCommand>()
                .ForMember(c => c.DeliveryModeIds, opt => opt.MapFrom(r => r.Ids));
        }
    }
}
