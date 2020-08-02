using AutoMapper;
using Sheaft.Domain.Models;
using Sheaft.Models.Dto;

namespace Sheaft.Mappers
{
    public class UserPointsProfile : Profile
    {
        public UserPointsProfile()
        {
            CreateMap<UserPoints, UserPointsDto>();
        }
    }
}
