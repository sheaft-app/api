using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Domain.Views;

namespace Sheaft.Application.Mappings
{
    public class CountryUserPointsProfile : Profile
    {
        public CountryUserPointsProfile()
        {
            CreateMap<CountryUserPoints, CountryUserPointsDto>();
        }
    }
}
