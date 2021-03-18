using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class NationalityViewProfile : Profile
    {
        public NationalityViewProfile()
        {
            CreateMap<Domain.Nationality, NationalityViewModel>()
                .ForMember(c => c.Code, opt => opt.MapFrom(t => t.Alpha2));

            CreateMap<NationalityViewModel, NationalityDto>();
            CreateMap<NationalityDto, NationalityViewModel>();
        }
    }
}
