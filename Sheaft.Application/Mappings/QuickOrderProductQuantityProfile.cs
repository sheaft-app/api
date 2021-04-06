using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Mappings
{
    public class QuickOrderProductQuantityProfile : Profile
    {
        public QuickOrderProductQuantityProfile()
        {
            CreateMap<QuickOrderProduct, QuickOrderProductQuantityDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(r => r.CatalogProduct.Product.Id))
                .ForMember(d => d.Reference, opt => opt.MapFrom(r => r.CatalogProduct.Product.Reference))
                .ForMember(d => d.Name, opt => opt.MapFrom(r => r.CatalogProduct.Product.Name))
                .ForMember(d => d.Returnable, opt => opt.MapFrom(r => r.CatalogProduct.Product.Returnable))
                .ForMember(d => d.UnitOnSalePrice, opt => opt.MapFrom(r => r.CatalogProduct.OnSalePricePerUnit))
                .ForMember(d => d.UnitWholeSalePrice, opt => opt.MapFrom(r => r.CatalogProduct.WholeSalePricePerUnit))
                .ForMember(d => d.UnitVatPrice, opt => opt.MapFrom(r => r.CatalogProduct.VatPricePerUnit));
        }
    }
}
