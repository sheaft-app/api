using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class CountryViewProfile : Profile
    {
        public CountryViewProfile()
        {
            CreateMap<Domain.Country, CountryViewModel>()
                .ForMember(c => c.Code, opt => opt.MapFrom(t => t.Alpha2));

            CreateMap<CountryDto, CountryViewModel>();
            CreateMap<CountryViewModel, CountryDto>();
        }
    }
}
