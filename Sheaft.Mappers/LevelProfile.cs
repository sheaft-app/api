using AutoMapper;
using Sheaft.Domain.Models;
using Sheaft.Models.Dto;

namespace Sheaft.Mappers
{
    public class LevelProfile : Profile
    {
        public LevelProfile()
        {
            CreateMap<Level, LevelDto>()
                .ForMember(d => d.Rewards, opt => opt.MapFrom(r => r.Rewards));
        }
    }
}
