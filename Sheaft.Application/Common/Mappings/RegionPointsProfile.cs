using AutoMapper;
using Sheaft.Domain.Views;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappers
{
    public class RegionPointsProfile : Profile
    {
        public RegionPointsProfile()
        {
            CreateMap<RegionPoints, RegionPointsDto>();
        }
    }
}
