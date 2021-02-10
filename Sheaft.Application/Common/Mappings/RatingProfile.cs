using AutoMapper;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Domain;

namespace Sheaft.Application.Common.Mappings
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
