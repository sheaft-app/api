using System;
using System.Linq;
using AutoMapper;
using Sheaft.Application.Extensions;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Domain.Product, ProductDto>()
                .ForMember(d => d.Tags, opt => opt.MapFrom(r => r.Tags.Select(t => t.Tag)))
                .ForMember(d => d.IsReturnable, opt => opt.MapFrom(r => r.Returnable != null))
                .ForMember(d => d.Picture,
                    opt => opt.MapFrom(r => PictureExtensions.GetPictureUrl(r.Id, r.Picture, PictureSize.LARGE)))
                .ForMember(d => d.ImageLarge,
                    opt => opt.MapFrom(r => PictureExtensions.GetPictureUrl(r.Id, r.Picture, PictureSize.LARGE)))
                .ForMember(d => d.ImageMedium,
                    opt => opt.MapFrom(r => PictureExtensions.GetPictureUrl(r.Id, r.Picture, PictureSize.MEDIUM)))
                .ForMember(d => d.ImageSmall,
                    opt => opt.MapFrom(r => PictureExtensions.GetPictureUrl(r.Id, r.Picture, PictureSize.SMALL)))
                .ForMember(d => d.VisibleToConsumers,
                    opt => opt.MapFrom(p => p.CatalogsPrices.Any(cp => cp.Catalog.Kind == CatalogKind.Consumers)))
                .ForMember(d => d.VisibleToStores,
                    opt => opt.MapFrom(p => p.CatalogsPrices.Any(cp => cp.Catalog.Kind == CatalogKind.Stores)))
                .ForMember(d => d.WholeSalePricePerUnit,
                    opt => opt.MapFrom(p => p.CatalogsPrices.FirstOrDefault().WholeSalePricePerUnit))
                .ForMember(d => d.VatPricePerUnit,
                    opt => opt.MapFrom(p => p.CatalogsPrices.FirstOrDefault().VatPricePerUnit))
                .ForMember(d => d.OnSalePricePerUnit,
                    opt => opt.MapFrom(p => p.CatalogsPrices.FirstOrDefault().OnSalePricePerUnit))
                .ForMember(d => d.WholeSalePrice,
                    opt => opt.MapFrom(p => p.CatalogsPrices.FirstOrDefault().WholeSalePrice))
                .ForMember(d => d.VatPrice,
                    opt => opt.MapFrom(p => p.CatalogsPrices.FirstOrDefault().VatPrice))
                .ForMember(d => d.OnSalePrice,
                    opt => opt.MapFrom(p => p.CatalogsPrices.FirstOrDefault().OnSalePrice));
        }
    }
}
