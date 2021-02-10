using AutoMapper;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Application.Common.Models.ViewModels;

namespace Sheaft.Application.Common.Mappings
{
    public class LevelProfile : Profile
    {
        public LevelProfile()
        {
            CreateMap<Domain.Level, LevelDto>()
                .ForMember(d => d.Rewards, opt => opt.MapFrom(r => r.Rewards));

            CreateMap<Domain.Level, LevelViewModel>()
                .ForMember(d => d.Rewards, opt => opt.MapFrom(r => r.Rewards));
        }
    }
}
