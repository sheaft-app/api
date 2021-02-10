using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Application.Common.Models.Inputs;
using Sheaft.Domain;

namespace Sheaft.Application.Common.Interfaces.Queries
{
    public interface IStoreQueries
    {
        IQueryable<StoreDto> GetStore(Guid id, RequestUser currentUser);
        Task<StoresSearchDto> SearchStoresAsync(Guid producerId, SearchTermsInput terms, RequestUser currentUser, CancellationToken token);
    }
}