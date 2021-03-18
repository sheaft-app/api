using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Domain.Views;

namespace Sheaft.Application.Mappings
{
    public class RegionPointsProfile : Profile
    {
        public RegionPointsProfile()
        {
            CreateMap<RegionPoints, RegionPointsDto>();
        }
    }
}
