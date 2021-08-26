using AutoMapper;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class RewardViewProfile : Profile
    {
        public RewardViewProfile()
        {
            CreateMap<Domain.Reward, RewardViewModel>();
            CreateMap<Domain.Reward, LevelRewardViewModel>();
        }
    }
}
