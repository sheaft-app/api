using AutoMapper;
using Sheaft.Domain.Models;
using Sheaft.Models.Dto;

namespace Sheaft.Mappers
{
    public class RewardProfile : Profile
    {
        public RewardProfile()
        {
            CreateMap<Reward, RewardDto>();
        }
    }
}
