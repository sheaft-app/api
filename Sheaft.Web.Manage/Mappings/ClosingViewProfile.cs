using AutoMapper;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class ClosingViewProfile : Profile
    {
        public ClosingViewProfile()
        {
            CreateMap<Domain.DeliveryClosing, ClosingViewModel>();
            CreateMap<Domain.BusinessClosing, ClosingViewModel>();
        }
    }
}