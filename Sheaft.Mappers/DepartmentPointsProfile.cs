using AutoMapper;
using Sheaft.Domain.Views;
using Sheaft.Models.Dto;

namespace Sheaft.Mappers
{
    public class DepartmentPointsProfile : Profile
    {
        public DepartmentPointsProfile()
        {
            CreateMap<DepartmentPoints, DepartmentPointsDto>();
        }
    }
}
