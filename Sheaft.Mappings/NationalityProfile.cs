using AutoMapper;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Application.Common.Models.ViewModels;

namespace Sheaft.Mappings
{
    public class NationalityProfile : Profile
    {
        public NationalityProfile()
        {
            CreateMap<Domain.Nationality, NationalityViewModel>();

            CreateMap<Domain.Nationality, NationalityDto>()
                .ForMember(c => c.Code, opt => opt.MapFrom(t => t.Alpha2));
        }
    }
}
