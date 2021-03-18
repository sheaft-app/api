using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class RegionViewProfile : Profile
    {
        public RegionViewProfile()
        {
            CreateMap<Domain.Region, RegionViewModel>();
            CreateMap<RegionDto, RegionViewModel>();
            CreateMap<RegionViewModel, RegionDto>();
        }
    }
}
