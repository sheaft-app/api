using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class DeliveryModeViewProfile : Profile
    {
        public DeliveryModeViewProfile()
        {
            CreateMap<Domain.DeliveryMode, AgreementDeliveryModeDto>();
            CreateMap<Domain.DeliveryMode, DeliveryModeViewModel>();
            CreateMap<DeliveryModeViewModel, AgreementDeliveryModeDto>();
            CreateMap<DeliveryModeViewModel, DeliveryModeDto>();
            CreateMap<DeliveryModeDto, DeliveryModeViewModel>();
            CreateMap<AgreementDeliveryModeDto, DeliveryModeViewModel>();
        }
    }
}
