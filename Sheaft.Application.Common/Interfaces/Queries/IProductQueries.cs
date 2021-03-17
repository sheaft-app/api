using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Application.Common.Models.Inputs;
using Sheaft.Domain;

namespace Sheaft.Application.Common.Interfaces.Queries
{
    public interface IProductQueries
    {
        Task<ProductsSearchDto> SearchAsync(SearchProductsInput terms, RequestUser currentUser, CancellationToken token);
        Task<bool> ProductIsRatedByUserAsync(Guid id, Guid userId, RequestUser user, CancellationToken token);
        IQueryable<ProductDto> GetProduct(Guid id, RequestUser currentUser);
        IQueryable<ProductDto> GetProducts(RequestUser currentUser);
        IQueryable<ProductDto> GetProducerProducts(Guid producerId, RequestUser currentUser);
    }
}