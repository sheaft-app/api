using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Domain.Views;

namespace Sheaft.Application.Mappings
{
    public class DepartmentUserPointsProfile : Profile
    {
        public DepartmentUserPointsProfile()
        {
            CreateMap<DepartmentUserPoints, DepartmentUserPointsDto>();
        }
    }
}
