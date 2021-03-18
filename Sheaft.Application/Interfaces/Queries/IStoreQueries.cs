using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Interfaces.Queries
{
    public interface IStoreQueries
    {
        IQueryable<StoreDto> GetStore(Guid id, RequestUser currentUser);
        Task<StoresSearchDto> SearchStoresAsync(Guid producerId, SearchTermsDto terms, RequestUser currentUser, CancellationToken token);
    }
}