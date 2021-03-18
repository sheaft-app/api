using AutoMapper;
using Sheaft.Application.Extensions;
using Sheaft.Application.Models;
using Sheaft.Core;

namespace Sheaft.Application.Mappings
{
    public class TagProfile : Profile
    {
        public TagProfile()
        {
            CreateMap<Domain.Tag, TagDto>()
                .ForMember(d => d.Picture, opt => opt.MapFrom(r => PictureExtensions.GetPictureUrl(r.Id, r.Picture, PictureSize.LARGE)));
        }
    }
}
