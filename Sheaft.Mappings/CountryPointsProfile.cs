using AutoMapper;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Domain.Views;

namespace Sheaft.Mappings
{
    public class CountryPointsProfile : Profile
    {
        public CountryPointsProfile()
        {
            CreateMap<CountryPoints, CountryPointsDto>();
        }
    }
}
