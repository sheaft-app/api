using AutoMapper;
using Sheaft.Domain.Models;
using Sheaft.Models.Dto;

namespace Sheaft.Mappers
{
    public class PointsProfile : Profile
    {
        public PointsProfile()
        {
            CreateMap<Points, PointsDto>();
        }
    }
}
