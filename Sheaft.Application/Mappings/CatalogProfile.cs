using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Mappings
{
    public class CatalogProfile : Profile
    {
        public CatalogProfile()
        {
            CreateMap<Catalog, CatalogDto>();
            CreateMap<Catalog, AgreementCatalogDto>();
            
            CreateMap<CatalogProduct, CatalogProductDto>()
                .ForMember(c => c.Id, opt => opt.MapFrom(e => e.Product.Id))
                .ForMember(c => c.Name, opt => opt.MapFrom(e => e.Product.Name))
                .ForMember(c => c.Reference, opt => opt.MapFrom(e => e.Product.Reference))
                .ForMember(c => c.AddedTo, opt => opt.MapFrom(e => e.CreatedOn));

            CreateMap<CatalogProduct, CatalogPriceDto>()
                .ForMember(c => c.Id, opt => opt.MapFrom(e => e.Catalog.Id))
                .ForMember(c => c.Name, opt => opt.MapFrom(e => e.Catalog.Name))
                .ForMember(c => c.AddedTo, opt => opt.MapFrom(e => e.CreatedOn));
        }
    }
}