using AutoMapper;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Application.Common.Models.ViewModels;

namespace Sheaft.Application.Common.Mappings
{
    public class RegionProfile : Profile
    {
        public RegionProfile()
        {
            CreateMap<Domain.Region, RegionDto>();
            CreateMap<Domain.Region, RegionViewModel>();
        }
    }
}
