using AutoMapper;
using Sheaft.Application.Commands;
using Sheaft.Core.Extensions;
using Sheaft.Core.Models;
using Sheaft.Domain.Models;
using Sheaft.Models.Dto;
using Sheaft.Models.Inputs;
using Sheaft.Models.ViewModels;
using System.Linq;

namespace Sheaft.Mappers
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(d => d.Producer, opt => opt.MapFrom(r => r.Producer))
                .ForMember(d => d.Tags, opt => opt.MapFrom(r => r.Tags.Select(t => t.Tag)))
                .ForMember(d => d.Packaged, opt => opt.MapFrom(r => r.Packaging != null))
                .ForMember(d => d.Packaging, opt => opt.MapFrom(r => r.Packaging))
                .ForMember(d => d.Picture, opt => opt.MapFrom(r => CoreProductExtensions.GetImageUrl(r.Image, ImageSize.LARGE)))
                .ForMember(d => d.ImageLarge, opt => opt.MapFrom(r => CoreProductExtensions.GetImageUrl(r.Image, ImageSize.LARGE)))
                .ForMember(d => d.ImageMedium, opt => opt.MapFrom(r => CoreProductExtensions.GetImageUrl(r.Image, ImageSize.MEDIUM)))
                .ForMember(d => d.ImageSmall, opt => opt.MapFrom(r => CoreProductExtensions.GetImageUrl(r.Image, ImageSize.SMALL)));

            CreateMap<Product, ProductViewModel>()
                .ForMember(d => d.Producer, opt => opt.MapFrom(r => r.Producer))
                .ForMember(d => d.Tags, opt => opt.MapFrom(r => r.Tags.Select(t => t.Tag.Id)))
                .ForMember(d => d.PackagingId, opt => opt.MapFrom(r => r.Packaging.Id))
                .ForMember(d => d.Picture, opt => opt.MapFrom(r => CoreProductExtensions.GetImageUrl(r.Image, ImageSize.LARGE)));

            CreateMap<CreateProductInput, CreateProductCommand>();
            CreateMap<UpdateProductInput, UpdateProductCommand>();
            CreateMap<RateProductInput, RateProductCommand>();
            CreateMap<UpdatePictureInput, UpdateProductPictureCommand>();
            CreateMap<SetProductsAvailabilityInput, SetProductsAvailabilityCommand>();
            CreateMap<IdsInput, DeleteProductsCommand>();
        }
    }
}
