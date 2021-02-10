using AutoMapper;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Domain.Views;

namespace Sheaft.Application.Common.Mappings
{
    public class RegionUserPointsProfile : Profile
    {
        public RegionUserPointsProfile()
        {
            CreateMap<RegionUserPoints, RegionUserPointsDto>();
        }
    }
}
