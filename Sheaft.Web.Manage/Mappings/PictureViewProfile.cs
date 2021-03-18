using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Domain;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class PictureViewProfile : Profile
    {
        public PictureViewProfile()
        {
            CreateMap<ProfilePicture, PictureViewModel>();
            CreateMap<ProductPicture, PictureViewModel>();
            
            CreateMap<PictureDto, PictureViewModel>();
            CreateMap<PictureViewModel, PictureDto>();
        }
    }
}