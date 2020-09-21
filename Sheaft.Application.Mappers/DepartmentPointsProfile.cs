using AutoMapper;
using Sheaft.Domain.Views;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappers
{
    public class DepartmentPointsProfile : Profile
    {
        public DepartmentPointsProfile()
        {
            CreateMap<DepartmentPoints, DepartmentPointsDto>();
        }
    }
}
