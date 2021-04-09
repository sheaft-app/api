using System.Linq;
using AutoMapper;
using Sheaft.Application.Extensions;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Domain.Enum;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class ProductViewProfile : Profile
    {
        public ProductViewProfile()
        {
            CreateMap<Domain.Product, ProductViewModel>()
                .ForMember(d => d.Tags, opt => opt.MapFrom(r => r.Tags.Select(t => t.Tag.Id)))
                .ForMember(d => d.Picture, opt => opt.MapFrom(r => PictureExtensions.GetPictureUrl(r.Id, r.Picture, PictureSize.LARGE)))
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

            CreateMap<ProductDto, ProductViewModel>();
            CreateMap<ProductViewModel, ProductDto>()
                .ForMember(d => d.IsReturnable, opt => opt.MapFrom(r => r.ReturnableId.HasValue));
        }
    }
}
