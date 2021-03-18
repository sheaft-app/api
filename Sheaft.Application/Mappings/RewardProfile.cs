using AutoMapper;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappings
{
    public class RewardProfile : Profile
    {
        public RewardProfile()
        {
            CreateMap<Domain.Reward, RewardDto>();
        }
    }
}
