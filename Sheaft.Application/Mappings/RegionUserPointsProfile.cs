using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Domain.Views;

namespace Sheaft.Application.Mappings
{
    public class RegionUserPointsProfile : Profile
    {
        public RegionUserPointsProfile()
        {
            CreateMap<RegionUserPoints, RegionUserPointsDto>();
        }
    }
}
