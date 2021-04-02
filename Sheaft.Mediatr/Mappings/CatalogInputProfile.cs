using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Mediatr.Catalog;

namespace Sheaft.Mediatr.Mappings
{
    public class CatalogInputProfile : Profile
    {
        public CatalogInputProfile()
        {
            CreateMap<CreateCatalogDto, CreateCatalogCommand>();
            CreateMap<UpdateCatalogDto, UpdateCatalogCommand>()
                .ForMember(c => c.CatalogId, opt =>opt.MapFrom(e => e.Id));
            CreateMap<ResourceIdsDto, DeleteCatalogsCommand>()
                .ForMember(c => c.CatalogIds, opt =>opt.MapFrom(e => e.Ids));
            CreateMap<ResourceIdDto, SetCatalogAsDefaultCommand>()
                .ForMember(c => c.CatalogId, opt =>opt.MapFrom(e => e.Id));
            CreateMap<SetResourceIdsAvailabilityDto, SetCatalogsAvailabilityCommand>()
                .ForMember(c => c.CatalogIds, opt =>opt.MapFrom(e => e.Ids));
            
            CreateMap<AddProductsToCatalogDto, AddProductsToCatalogCommand>()
                .ForMember(c => c.CatalogId, opt =>opt.MapFrom(e => e.Id));
            CreateMap<RemoveProductsFromCatalogDto, RemoveProductsFromCatalogCommand>()
                .ForMember(c => c.CatalogId, opt =>opt.MapFrom(e => e.Id));
            
            CreateMap<CloneCatalogDto, CloneCatalogCommand>()
                .ForMember(c => c.CatalogId, opt =>opt.MapFrom(e => e.Id));
            CreateMap<UpdateAllCatalogPricesDto, UpdateAllCatalogPricesCommand>()
                .ForMember(c => c.CatalogId, opt =>opt.MapFrom(e => e.Id));
            CreateMap<UpdateCatalogPricesDto, UpdateCatalogPricesCommand>()
                .ForMember(c => c.CatalogId, opt =>opt.MapFrom(e => e.Id));
        }
    }
}