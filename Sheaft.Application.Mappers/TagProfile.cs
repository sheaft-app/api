using AutoMapper;
using Sheaft.Domain.Models;
using Sheaft.Application.Models;
using Sheaft.Core.Extensions;
using Sheaft.Core.Models;

namespace Sheaft.Application.Mappers
{
    public class TagProfile : Profile
    {
        public TagProfile()
        {
            CreateMap<Tag, TagDto>()
                .ForMember(d => d.Picture, opt => opt.MapFrom(r => CoreProductExtensions.GetPictureUrl(r.Picture, PictureSize.LARGE)));
            CreateMap<Tag, TagViewModel>()
                .ForMember(d => d.Picture, opt => opt.MapFrom(r => CoreProductExtensions.GetPictureUrl(r.Picture, PictureSize.LARGE)));
        }
    }
}
