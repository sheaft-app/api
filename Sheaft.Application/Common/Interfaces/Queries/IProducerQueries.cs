using Sheaft.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application.Models;

namespace Sheaft.Application.Queries
{
    public interface IProducerQueries
    {
        IQueryable<T> GetProducer<T>(Guid id, RequestUser currentUser);
        Task<ProducersSearchDto> SearchProducersAsync(Guid storeId, SearchTermsInput terms, RequestUser currentUser, CancellationToken token);
        Task<IEnumerable<ProducerSuggestDto>> SuggestProducersAsync(SearchTermsInput terms, RequestUser currentUser, CancellationToken token);
        IQueryable<ProducerSummaryDto> GetProducers(RequestUser currentUser);
    }
}