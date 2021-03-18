using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Interfaces.Queries
{
    public interface IProductQueries
    {
        Task<ProductsSearchDto> SearchAsync(SearchProductsDto terms, RequestUser currentUser, CancellationToken token);
        Task<bool> ProductIsRatedByUserAsync(Guid id, Guid userId, RequestUser user, CancellationToken token);
        IQueryable<ProductDto> GetProduct(Guid id, RequestUser currentUser);
        IQueryable<ProductDto> GetProducts(RequestUser currentUser);
        IQueryable<ProductDto> GetProducerProducts(Guid producerId, RequestUser currentUser);
    }
}