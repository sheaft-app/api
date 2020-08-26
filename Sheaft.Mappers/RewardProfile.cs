using AutoMapper;
using Sheaft.Domain.Models;
using Sheaft.Models.Dto;
using Sheaft.Models.ViewModels;

namespace Sheaft.Mappers
{
    public class RewardProfile : Profile
    {
        public RewardProfile()
        {
            CreateMap<Reward, RewardDto>();
            CreateMap<Reward, RewardViewModel>()
                .ForMember(d => d.DepartmentId, opt => opt.MapFrom(r => r.Department.Id))
                .ForMember(d => d.LevelId, opt => opt.MapFrom(r => r.Level.Id));

            CreateMap<Reward, LevelRewardViewModel>()
                .ForMember(d => d.Department, opt => opt.MapFrom(r => r.Department.Name));
        }
    }
}
