using AutoMapper;
using Sheaft.Application.Commands;
using Sheaft.Core.Extensions;
using Sheaft.Core.Models;
using Sheaft.Domain.Models;
using Sheaft.Application.Models;
using System.Linq;

namespace Sheaft.Application.Mappers
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(d => d.Producer, opt => opt.MapFrom(r => r.Producer))
                .ForMember(d => d.Tags, opt => opt.MapFrom(r => r.Tags.Select(t => t.Tag)))
                .ForMember(d => d.IsReturnable, opt => opt.MapFrom(r => r.Returnable != null))
                .ForMember(d => d.Returnable, opt => opt.MapFrom(r => r.Returnable))
                .ForMember(d => d.Picture, opt => opt.MapFrom(r => CoreProductExtensions.GetPictureUrl(r.Picture, PictureSize.LARGE)))
                .ForMember(d => d.ImageLarge, opt => opt.MapFrom(r => CoreProductExtensions.GetPictureUrl(r.Picture, PictureSize.LARGE)))
                .ForMember(d => d.ImageMedium, opt => opt.MapFrom(r => CoreProductExtensions.GetPictureUrl(r.Picture, PictureSize.MEDIUM)))
                .ForMember(d => d.ImageSmall, opt => opt.MapFrom(r => CoreProductExtensions.GetPictureUrl(r.Picture, PictureSize.SMALL)));

            CreateMap<Product, ProductViewModel>()
                .ForMember(d => d.Producer, opt => opt.MapFrom(r => r.Producer))
                .ForMember(d => d.Tags, opt => opt.MapFrom(r => r.Tags.Select(t => t.Tag.Id)))
                .ForMember(d => d.ReturnableId, opt => opt.MapFrom(r => r.Returnable.Id))
                .ForMember(d => d.Picture, opt => opt.MapFrom(r => CoreProductExtensions.GetPictureUrl(r.Picture, PictureSize.LARGE)));

            CreateMap<CreateProductInput, CreateProductCommand>();
            CreateMap<UpdateProductInput, UpdateProductCommand>();
            CreateMap<RateProductInput, RateProductCommand>();
            CreateMap<UpdatePictureInput, UpdateProductPictureCommand>();
            CreateMap<SetProductsAvailabilityInput, SetProductsAvailabilityCommand>();
            CreateMap<SetProductsSearchabilityInput, SetProductsSearchabilityCommand>();
            CreateMap<IdsInput, DeleteProductsCommand>();
        }
    }
}
