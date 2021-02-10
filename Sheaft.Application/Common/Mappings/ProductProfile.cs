﻿using System.Linq;
using AutoMapper;
using Sheaft.Application.Common.Extensions;
using Sheaft.Application.Common.Models;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Application.Common.Models.Inputs;
using Sheaft.Application.Common.Models.ViewModels;
using Sheaft.Application.Picture.Commands;
using Sheaft.Application.Product.Commands;

namespace Sheaft.Application.Common.Mappings
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
                .ForMember(d => d.Picture, opt => opt.MapFrom(r => ProductExtensions.GetPictureUrl(r.Picture, PictureSize.LARGE)))
                .ForMember(d => d.ImageLarge, opt => opt.MapFrom(r => ProductExtensions.GetPictureUrl(r.Picture, PictureSize.LARGE)))
                .ForMember(d => d.ImageMedium, opt => opt.MapFrom(r => ProductExtensions.GetPictureUrl(r.Picture, PictureSize.MEDIUM)))
                .ForMember(d => d.ImageSmall, opt => opt.MapFrom(r => ProductExtensions.GetPictureUrl(r.Picture, PictureSize.SMALL)));

            CreateMap<Domain.Product, ProductViewModel>()
                .ForMember(d => d.Producer, opt => opt.MapFrom(r => r.Producer))
                .ForMember(d => d.Tags, opt => opt.MapFrom(r => r.Tags.Select(t => t.Tag.Id)))
                .ForMember(d => d.ReturnableId, opt => opt.MapFrom(r => r.Returnable.Id))
                .ForMember(d => d.Picture, opt => opt.MapFrom(r => ProductExtensions.GetPictureUrl(r.Picture, PictureSize.LARGE)));

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
