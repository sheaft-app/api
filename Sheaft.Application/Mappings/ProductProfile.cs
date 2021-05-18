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
                .ForMember(d => d.WholeSalePricePerUnit,
                    opt => opt.MapFrom(p => p.CatalogsPrices.OrderByDescending(cp => cp.Catalog.Kind).FirstOrDefault().WholeSalePricePerUnit))
                .ForMember(d => d.VatPricePerUnit,
                    opt => opt.MapFrom(p => p.CatalogsPrices.OrderByDescending(cp => cp.Catalog.Kind).FirstOrDefault().VatPricePerUnit))
                .ForMember(d => d.OnSalePricePerUnit,
                    opt => opt.MapFrom(p => p.CatalogsPrices.OrderByDescending(cp => cp.Catalog.Kind).FirstOrDefault().OnSalePricePerUnit))
                .ForMember(d => d.WholeSalePrice,
                    opt => opt.MapFrom(p => p.CatalogsPrices.OrderByDescending(cp => cp.Catalog.Kind).FirstOrDefault().WholeSalePrice))
                .ForMember(d => d.VatPrice,
                    opt => opt.MapFrom(p => p.CatalogsPrices.OrderByDescending(cp => cp.Catalog.Kind).FirstOrDefault().VatPrice))
                .ForMember(d => d.OnSalePrice,
                    opt => opt.MapFrom(p => p.CatalogsPrices.OrderByDescending(cp => cp.Catalog.Kind).FirstOrDefault().OnSalePrice));
        }
    }
}
