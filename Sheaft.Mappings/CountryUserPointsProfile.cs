using AutoMapper;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Domain.Views;

namespace Sheaft.Mappings
{
    public class CountryUserPointsProfile : Profile
    {
        public CountryUserPointsProfile()
        {
            CreateMap<CountryUserPoints, CountryUserPointsDto>();
        }
    }
}
