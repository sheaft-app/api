using AutoMapper;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Domain.Views;

namespace Sheaft.Application.Common.Mappings
{
    public class CountryUserPointsProfile : Profile
    {
        public CountryUserPointsProfile()
        {
            CreateMap<CountryUserPoints, CountryUserPointsDto>();
        }
    }
}
