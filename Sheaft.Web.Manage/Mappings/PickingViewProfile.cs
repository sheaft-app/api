using AutoMapper;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class PickingViewProfile : Profile
    {
        public PickingViewProfile()
        {
            CreateMap<Domain.Picking, PickingViewModel>();
            CreateMap<Domain.Picking, ShortPickingViewModel>();
        }
    }
}