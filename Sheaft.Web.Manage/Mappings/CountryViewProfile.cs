using AutoMapper;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class CountryViewProfile : Profile
    {
        public CountryViewProfile()
        {
            CreateMap<Domain.Country, CountryViewModel>()
                .ForMember(c => c.Code, opt => opt.MapFrom(t => t.Alpha2));
        }
    }
}
