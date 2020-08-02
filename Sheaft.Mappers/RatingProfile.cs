using AutoMapper;
using Sheaft.Domain.Models;
using Sheaft.Models.Dto;

namespace Sheaft.Mappers
{
    public class RatingProfile : Profile
    {
        public RatingProfile()
        {
            CreateMap<Rating, RatingDto>()
                .ForMember(d => d.User, opt => opt.MapFrom(r => r.User));
        }
    }
}
