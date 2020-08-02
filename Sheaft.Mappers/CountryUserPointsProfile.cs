using AutoMapper;
using Sheaft.Domain.Views;
using Sheaft.Models.Dto;

namespace Sheaft.Mappers
{
    public class CountryUserPointsProfile : Profile
    {
        public CountryUserPointsProfile()
        {
            CreateMap<CountryUserPoints, CountryUserPointsDto>();
        }
    }
}
