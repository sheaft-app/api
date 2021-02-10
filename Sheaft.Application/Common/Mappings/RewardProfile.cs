using AutoMapper;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Application.Common.Models.ViewModels;

namespace Sheaft.Application.Common.Mappings
{
    public class RewardProfile : Profile
    {
        public RewardProfile()
        {
            CreateMap<Domain.Reward, RewardDto>();
            CreateMap<Domain.Reward, RewardViewModel>()
                .ForMember(d => d.DepartmentId, opt => opt.MapFrom(r => r.Department.Id))
                .ForMember(d => d.LevelId, opt => opt.MapFrom(r => r.Level.Id));

            CreateMap<Domain.Reward, LevelRewardViewModel>()
                .ForMember(d => d.Department, opt => opt.MapFrom(r => r.Department.Name));
        }
    }
}
