using System;
using System.Linq;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Interfaces.Queries
{
    public interface ICatalogQueries
    {
        IQueryable<CatalogDto> GetCatalog(Guid id, RequestUser currentUser);
        IQueryable<CatalogProductDto> GetCatalogProducts(Guid id, RequestUser currentUser);
        IQueryable<CatalogDto> GetCatalogs(RequestUser currentUser);
    }
}