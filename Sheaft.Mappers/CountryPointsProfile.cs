using AutoMapper;
using Sheaft.Domain.Views;
using Sheaft.Models.Dto;

namespace Sheaft.Mappers
{
    public class CountryPointsProfile : Profile
    {
        public CountryPointsProfile()
        {
            CreateMap<CountryPoints, CountryPointsDto>();
        }
    }
}
