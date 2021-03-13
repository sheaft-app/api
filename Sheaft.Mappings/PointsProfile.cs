using AutoMapper;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Domain;

namespace Sheaft.Mappings
{
    public class PointsProfile : Profile
    {
        public PointsProfile()
        {
            CreateMap<Points, PointsDto>();
        }
    }
}
