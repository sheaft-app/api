using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application.Models;
using Sheaft.Core;

namespace Sheaft.Application.Queries
{
    public interface IStoreQueries
    {
        IQueryable<StoreDto> GetStore(Guid id, RequestUser currentUser);
        Task<StoresSearchDto> SearchStoresAsync(Guid producerId, SearchTermsInput terms, RequestUser currentUser, CancellationToken token);
    }
}