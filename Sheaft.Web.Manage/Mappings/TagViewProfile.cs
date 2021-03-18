using AutoMapper;
using Sheaft.Application.Extensions;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class TagViewProfile : Profile
    {
        public TagViewProfile()
        {
            CreateMap<Domain.Tag, TagViewModel>()
                .ForMember(d => d.Picture, opt => opt.MapFrom(r => PictureExtensions.GetPictureUrl(r.Id, r.Picture, PictureSize.LARGE)));

            CreateMap<TagViewModel, TagDto>();
            CreateMap<TagDto, TagViewModel>();
        }
    }
}
