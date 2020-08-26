using AutoMapper;
using Sheaft.Domain.Models;
using Sheaft.Models.Dto;
using Sheaft.Models.ViewModels;

namespace Sheaft.Mappers
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
