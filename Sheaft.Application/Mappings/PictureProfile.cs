using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Mappings
{
    public class PictureProfile : Profile
    {
        public PictureProfile()
        {
            CreateMap<ProductPicture, PictureDto>();
            CreateMap<ProfilePicture, PictureDto>();
        }
    }
}