using AutoMapper;
using Sheaft.Application.Extensions;
using Sheaft.Core;
using Sheaft.Domain;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class PictureViewProfile : Profile
    {
        public PictureViewProfile()
        {
            CreateMap<ProfilePicture, PictureViewModel>()
                .ForMember(c => c.Url, opt => opt.MapFrom(e => PictureExtensions.GetPictureUrl(e.UserId, e.Url, PictureSize.MEDIUM)));
            CreateMap<ProductPicture, PictureViewModel>()
                .ForMember(c => c.Url, opt => opt.MapFrom(e => PictureExtensions.GetPictureUrl(e.ProductId, e.Url, PictureSize.MEDIUM)));
        }
    }
}