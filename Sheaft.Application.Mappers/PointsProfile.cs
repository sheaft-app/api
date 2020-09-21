using AutoMapper;
using Sheaft.Domain.Models;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappers
{
    public class PointsProfile : Profile
    {
        public PointsProfile()
        {
            CreateMap<Points, PointsDto>();
        }
    }
}
