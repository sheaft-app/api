using AutoMapper;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Domain.Views;

namespace Sheaft.Mappings
{
    public class DepartmentPointsProfile : Profile
    {
        public DepartmentPointsProfile()
        {
            CreateMap<DepartmentPoints, DepartmentPointsDto>();
        }
    }
}
