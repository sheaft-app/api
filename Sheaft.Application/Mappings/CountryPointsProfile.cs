using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Domain.Views;

namespace Sheaft.Application.Mappings
{
    public class CountryPointsProfile : Profile
    {
        public CountryPointsProfile()
        {
            CreateMap<CountryPoints, CountryPointsDto>();
        }
    }
}
