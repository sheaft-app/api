using System.Linq;
using AutoMapper;
using Sheaft.Application.Extensions;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class ProductViewProfile : Profile
    {
        public ProductViewProfile()
        {
            CreateMap<Domain.Product, ProductViewModel>()
                .ForMember(d => d.Tags, opt => opt.MapFrom(r => r.Tags.Select(t => t.Tag.Id)))
                .ForMember(d => d.Picture, opt => opt.MapFrom(r => PictureExtensions.GetPictureUrl(r.Id, r.Picture, PictureSize.LARGE)));

            CreateMap<ProductDto, ProductViewModel>();
            CreateMap<ProductViewModel, ProductDto>()
                .ForMember(d => d.IsReturnable, opt => opt.MapFrom(r => r.ReturnableId.HasValue));
        }
    }
}
