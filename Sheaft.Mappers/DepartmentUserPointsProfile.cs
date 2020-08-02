using AutoMapper;
using Sheaft.Domain.Views;
using Sheaft.Models.Dto;

namespace Sheaft.Mappers
{
    public class DepartmentUserPointsProfile : Profile
    {
        public DepartmentUserPointsProfile()
        {
            CreateMap<DepartmentUserPoints, DepartmentUserPointsDto>();
        }
    }
}
