using AutoMapper;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Domain;

namespace Sheaft.Application.Common.Mappings
{
    public class PictureProfile : Profile
    {
        public PictureProfile()
        {
            CreateMap<ProfilePicture, PictureDto>();
            CreateMap<ProductPicture, PictureDto>();
        }
    }
}