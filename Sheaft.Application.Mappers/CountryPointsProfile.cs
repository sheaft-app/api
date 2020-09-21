using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Domain.Views;

namespace Sheaft.Application.Mappers
{
    public class CountryPointsProfile : Profile
    {
        public CountryPointsProfile()
        {
            CreateMap<CountryPoints, CountryPointsDto>();
        }
    }
}
