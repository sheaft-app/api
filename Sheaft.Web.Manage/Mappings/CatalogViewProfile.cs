using AutoMapper;
using Sheaft.Application.Models;
using Sheaft.Web.Manage.Models;

namespace Sheaft.Web.Manage.Mappings
{
    public class CatalogViewProfile : Profile
    {
        public CatalogViewProfile()
        {
            CreateMap<Domain.Catalog, CatalogViewModel>();
            CreateMap<Domain.Catalog, ShortCatalogViewModel>();
        }
    }
}