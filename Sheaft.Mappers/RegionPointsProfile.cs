using AutoMapper;
using Sheaft.Domain.Views;
using Sheaft.Models.Dto;

namespace Sheaft.Mappers
{
    public class RegionPointsProfile : Profile
    {
        public RegionPointsProfile()
        {
            CreateMap<RegionPoints, RegionPointsDto>();
        }
    }
}
