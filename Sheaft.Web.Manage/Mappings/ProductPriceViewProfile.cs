using AutoMapper;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class ProductPriceViewProfile : Profile
    {
        public ProductPriceViewProfile()
        {
            CreateMap<Domain.CatalogProduct, ProductPriceViewModel>()
                .ForMember(c => c.Id, opt => opt.MapFrom(r => r.ProductId))
                .ForMember(c => c.Name, opt => opt.MapFrom(r => r.Product.Name))
                .ForMember(c => c.Reference, opt => opt.MapFrom(r => r.Product.Reference))
                .ForMember(c => c.WholeSalePricePerUnit, opt => opt.MapFrom(r => r.WholeSalePricePerUnit));
        }
    }
}