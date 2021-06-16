using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Domain;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class ExpectedOrderDeliveryViewProfile : Profile
    {
        public ExpectedOrderDeliveryViewProfile()
        {
            CreateMap<ExpectedOrderDeliveryDto, ExpectedOrderDeliveryViewModel>();
            CreateMap<ExpectedOrderDeliveryViewModel, ExpectedOrderDeliveryDto>();
        }
    }
}
