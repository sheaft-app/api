using AutoMapper;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class NationalityViewProfile : Profile
    {
        public NationalityViewProfile()
        {
            CreateMap<Domain.Nationality, NationalityViewModel>()
                .ForMember(c => c.Code, opt => opt.MapFrom(t => t.Alpha2));
        }
    }
}
