using AutoMapper;
using Sheaft.Domain.Views;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappers
{
    public class RegionUserPointsProfile : Profile
    {
        public RegionUserPointsProfile()
        {
            CreateMap<RegionUserPoints, RegionUserPointsDto>();
        }
    }
}
