using AutoMapper;
using Sheaft.Domain.Models;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappers
{
    public class RegionProfile : Profile
    {
        public RegionProfile()
        {
            CreateMap<Region, RegionDto>();
            CreateMap<Region, RegionViewModel>();
        }
    }
}
