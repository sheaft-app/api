using AutoMapper;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Domain.Views;

namespace Sheaft.Mappings
{
    public class RegionUserPointsProfile : Profile
    {
        public RegionUserPointsProfile()
        {
            CreateMap<RegionUserPoints, RegionUserPointsDto>();
        }
    }
}
