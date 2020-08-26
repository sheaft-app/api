using AutoMapper;
using Sheaft.Domain.Models;
using Sheaft.Models.Dto;
using Sheaft.Models.ViewModels;

namespace Sheaft.Mappers
{

    public class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            CreateMap<Department, DepartmentDto>()
                .ForMember(c => c.Level, opt => opt.MapFrom(o => o.Level));

            CreateMap<Department, DepartmentViewModel>()
                .ForMember(c => c.Level, opt => opt.MapFrom(o => $"{o.Level.Number} - {o.Level.Name}"));
        }
    }
}
