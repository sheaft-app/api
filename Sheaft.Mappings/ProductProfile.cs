using System.Linq;
using AutoMapper;
using Sheaft.Application.Common.Extensions;
using Sheaft.Application.Common.Models;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Application.Common.Models.Inputs;
using Sheaft.Application.Common.Models.ViewModels;
using Sheaft.Application.Product.Commands;

namespace Sheaft.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Domain.Product, ProductDto>()
                .ForMember(d => d.Producer, opt => opt.MapFrom(r => r.Producer))
                .ForMember(d => d.Tags, opt => opt.MapFrom(r => r.Tags.Select(t => t.Tag)))
                .ForMember(d => d.IsReturnable, opt => opt.MapFrom(r => r.Returnable != null))
                .ForMember(d => d.Returnable, opt => opt.MapFrom(r => r.Returnable))
                .ForMember(d => d.Picture, opt => opt.MapFrom(r => PictureExtensions.GetPictureUrl(r.Id, r.Picture, PictureSize.LARGE)))
                .ForMember(d => d.ImageLarge, opt => opt.MapFrom(r => PictureExtensions.GetPictureUrl(r.Id, r.Picture, PictureSize.LARGE)))
                .ForMember(d => d.ImageMedium, opt => opt.MapFrom(r => PictureExtensions.GetPictureUrl(r.Id, r.Picture, PictureSize.MEDIUM)))
                .ForMember(d => d.ImageSmall, opt => opt.MapFrom(r => PictureExtensions.GetPictureUrl(r.Id, r.Picture, PictureSize.SMALL)));

            CreateMap<Domain.Product, ProductViewModel>()
                .ForMember(d => d.Producer, opt => opt.MapFrom(r => r.Producer))
                .ForMember(d => d.Tags, opt => opt.MapFrom(r => r.Tags.Select(t => t.Tag.Id)))
                .ForMember(d => d.ReturnableId, opt => opt.MapFrom(r => r.Returnable.Id))
                .ForMember(d => d.Picture, opt => opt.MapFrom(r => PictureExtensions.GetPictureUrl(r.Id, r.Picture, PictureSize.LARGE)));

            CreateMap<AddPictureToInput, AddPictureToProductCommand>()
                .ForMember(c => c.ProductId, opt => opt.MapFrom(r => r.Id));

            CreateMap<IdInput, RemoveProductPictureCommand>();
            CreateMap<IdsInput, RemoveProductPicturesCommand>();
            
            CreateMap<CreateProductInput, CreateProductCommand>();
            CreateMap<UpdateProductInput, UpdateProductCommand>()
                .ForMember(c => c.ProductId, opt => opt.MapFrom(r => r.Id));
            CreateMap<RateProductInput, RateProductCommand>()
                .ForMember(c => c.ProductId, opt => opt.MapFrom(r => r.Id));
            CreateMap<UpdatePictureInput, UpdateProductPreviewCommand>()
                .ForMember(c => c.ProductId, opt => opt.MapFrom(r => r.Id));
            CreateMap<SetProductsAvailabilityInput, SetProductsAvailabilityCommand>()
                .ForMember(c => c.ProductIds, opt => opt.MapFrom(r => r.Ids));
            CreateMap<SetProductsSearchabilityInput, SetProductsSearchabilityCommand>()
                .ForMember(c => c.ProductIds, opt => opt.MapFrom(r => r.Ids));
            CreateMap<IdsInput, DeleteProductsCommand>()
                .ForMember(c => c.ProductIds, opt => opt.MapFrom(r => r.Ids));;
        }
    }
}
