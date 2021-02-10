using AutoMapper;
using Sheaft.Domain.Models;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappers
{
    public class CountryProfile : Profile
    {
        public CountryProfile()
        {
            CreateMap<Country, CountryViewModel>();

            CreateMap<Country, CountryDto>()
                .ForMember(c => c.Code, opt => opt.MapFrom(t => t.Alpha2));
        }
    }
}
