using AutoMapper;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class RegionViewProfile : Profile
    {
        public RegionViewProfile()
        {
            CreateMap<Domain.Region, RegionViewModel>();
        }
    }
}
