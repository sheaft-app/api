using AutoMapper;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Application.Common.Models.ViewModels;

namespace Sheaft.Application.Common.Mappings
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            CreateMap<Domain.Department, DepartmentDto>()
                .ForMember(c => c.Level, opt => opt.MapFrom(o => o.Level));

            CreateMap<Domain.Department, DepartmentViewModel>()
                .ForMember(c => c.Level, opt => opt.MapFrom(o => o.Level.Name));
        }
    }
}
