using AutoMapper;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class CatalogPriceViewProfile : Profile
    {
        public CatalogPriceViewProfile()
        {
            CreateMap<Domain.CatalogProduct, CatalogPriceViewModel>()
                .ForMember(c => c.Id, opt => opt.MapFrom(r => r.CatalogId))
                .ForMember(c => c.Name, opt => opt.MapFrom(r => r.Catalog.Name))
                .ForMember(c => c.WholeSalePricePerUnit, opt => opt.MapFrom(r => r.WholeSalePricePerUnit));
        }
    }
}