using AutoMapper;
using Sheaft.Domain.Models;
using Sheaft.Models.Dto;

namespace Sheaft.Mappers
{
    public class NationalityProfile : Profile
    {
        public NationalityProfile()
        {
            CreateMap<Nationality, NationalityDto>()
                .ForMember(c => c.Code, opt => opt.MapFrom(t => t.Alpha2));
        }
    }
}
