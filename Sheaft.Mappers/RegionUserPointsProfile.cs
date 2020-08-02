using AutoMapper;
using Sheaft.Domain.Views;
using Sheaft.Models.Dto;

namespace Sheaft.Mappers
{
    public class RegionUserPointsProfile : Profile
    {
        public RegionUserPointsProfile()
        {
            CreateMap<RegionUserPoints, RegionUserPointsDto>();
        }
    }
}
