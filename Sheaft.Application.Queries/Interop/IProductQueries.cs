using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Models.Dto;
using Sheaft.Models.Inputs;
using Sheaft.Interop;

namespace Sheaft.Application.Queries
{
    public interface IProductQueries
    {
        Task<ProductsSearchDto> SearchAsync(SearchTermsInput terms, IRequestUser currentUser, CancellationToken token);
        Task<bool> ProductIsRatedByUserAsync(Guid id, Guid userId, IRequestUser user, CancellationToken token);
        IQueryable<ProductDto> GetStoreProducts(Guid storeId, IRequestUser currentUser);
        IQueryable<ProductDto> GetProducerProducts(Guid producerId, IRequestUser currentUser);
        IQueryable<ProductDto> GetProduct(Guid id, IRequestUser currentUser);
        IQueryable<ProductDto> GetProducts(IRequestUser currentUser);
    }
}