using AutoMapper;
using Sheaft.Application.Common.Extensions;
using Sheaft.Application.Common.Models;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Application.Common.Models.ViewModels;

namespace Sheaft.Application.Common.Mappings
{
    public class TagProfile : Profile
    {
        public TagProfile()
        {
            CreateMap<Domain.Tag, TagDto>()
                .ForMember(d => d.Picture, opt => opt.MapFrom(r => ProductExtensions.GetPictureUrl(r.Picture, PictureSize.LARGE)));
            CreateMap<Domain.Tag, TagViewModel>()
                .ForMember(d => d.Picture, opt => opt.MapFrom(r => ProductExtensions.GetPictureUrl(r.Picture, PictureSize.LARGE)));
        }
    }
}
