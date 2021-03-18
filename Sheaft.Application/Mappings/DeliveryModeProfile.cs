using AutoMapper;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappings
{
    public class DeliveryModeProfile : Profile
    {
        public DeliveryModeProfile()
        {
            CreateMap<Domain.DeliveryMode, DeliveryModeDto>();
            CreateMap<Domain.DeliveryMode, AgreementDeliveryModeDto>();
        }
    }
}
