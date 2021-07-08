using System.Linq;
using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Domain.Enum;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class DeliveryModeViewProfile : Profile
    {
        public DeliveryModeViewProfile()
        {
            CreateMap<Domain.DeliveryMode, DeliveryModeViewModel>()
                .ForMember(m => m.Agreements, opt => opt.MapFrom(e => e.Agreements.Where(a => a.Status == AgreementStatus.Accepted)));
            CreateMap<Domain.DeliveryMode, ShortDeliveryModeViewModel>();
        }
    }
}
