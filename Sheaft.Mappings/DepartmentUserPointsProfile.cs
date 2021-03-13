using AutoMapper;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Domain.Views;

namespace Sheaft.Mappings
{
    public class DepartmentUserPointsProfile : Profile
    {
        public DepartmentUserPointsProfile()
        {
            CreateMap<DepartmentUserPoints, DepartmentUserPointsDto>();
        }
    }
}
