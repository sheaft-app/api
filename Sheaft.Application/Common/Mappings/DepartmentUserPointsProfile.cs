using AutoMapper;
using Sheaft.Domain.Views;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappers
{
    public class DepartmentUserPointsProfile : Profile
    {
        public DepartmentUserPointsProfile()
        {
            CreateMap<DepartmentUserPoints, DepartmentUserPointsDto>();
        }
    }
}
