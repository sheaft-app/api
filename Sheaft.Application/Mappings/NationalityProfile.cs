using AutoMapper;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappings
{
    public class NationalityProfile : Profile
    {
        public NationalityProfile()
        {
            CreateMap<Domain.Nationality, NationalityDto>()
                .ForMember(c => c.Code, opt => opt.MapFrom(t => t.Alpha2));
        }
    }
}
