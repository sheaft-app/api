using AutoMapper;
using Sheaft.Domain.Models;
using Sheaft.Models.Dto;
using Sheaft.Models.ViewModels;
using System.Linq;

namespace Sheaft.Mappers
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
