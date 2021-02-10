using AutoMapper;
using Sheaft.Domain.Views;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappers
{
    public class CountryUserPointsProfile : Profile
    {
        public CountryUserPointsProfile()
        {
            CreateMap<CountryUserPoints, CountryUserPointsDto>();
        }
    }
}
