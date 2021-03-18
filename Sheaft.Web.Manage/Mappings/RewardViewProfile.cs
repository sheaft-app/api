using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class RewardViewProfile : Profile
    {
        public RewardViewProfile()
        {
            CreateMap<Domain.Reward, RewardViewModel>();
            CreateMap<Domain.Reward, LevelRewardViewModel>();
            
            CreateMap<RewardDto, RewardViewModel>();
            CreateMap<RewardDto, LevelRewardViewModel>();
            CreateMap<RewardViewModel, RewardDto>();
            CreateMap<LevelRewardViewModel, RewardDto>();
        }
    }
}
