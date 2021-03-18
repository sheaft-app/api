using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Domain.Views;

namespace Sheaft.Application.Mappings
{
    public class DepartmentPointsProfile : Profile
    {
        public DepartmentPointsProfile()
        {
            CreateMap<DepartmentPoints, DepartmentPointsDto>();
        }
    }
}
