using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application.Models;
using Sheaft.Core;

namespace Sheaft.Application.Queries
{
    public interface IProductQueries
    {
        Task<ProductsSearchDto> SearchAsync(SearchProductsInput terms, RequestUser currentUser, CancellationToken token);
        Task<bool> ProductIsRatedByUserAsync(Guid id, Guid userId, RequestUser user, CancellationToken token);
        IQueryable<ProductDto> GetStoreProducts(Guid storeId, RequestUser currentUser);
        IQueryable<ProductDto> GetProducerProductsForStores(Guid producerId, RequestUser currentUser);
        IQueryable<ProductDto> GetProduct(Guid id, RequestUser currentUser);
        IQueryable<ProductDto> GetProducts(RequestUser currentUser);
    }
}