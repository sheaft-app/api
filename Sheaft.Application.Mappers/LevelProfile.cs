using AutoMapper;
using Sheaft.Domain.Models;
using Sheaft.Application.Models;

namespace Sheaft.Application.Mappers
{
    public class LevelProfile : Profile
    {
        public LevelProfile()
        {
            CreateMap<Level, LevelDto>()
                .ForMember(d => d.Rewards, opt => opt.MapFrom(r => r.Rewards));

            CreateMap<Level, LevelViewModel>()
                .ForMember(d => d.Rewards, opt => opt.MapFrom(r => r.Rewards));
        }
    }
}
