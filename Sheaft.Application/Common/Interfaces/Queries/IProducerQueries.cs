using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Application.Common.Models.Inputs;
using Sheaft.Domain;

namespace Sheaft.Application.Common.Interfaces.Queries
{
    public interface IProducerQueries
    {
        IQueryable<T> GetProducer<T>(Guid id, RequestUser currentUser);
        Task<ProducersSearchDto> SearchProducersAsync(Guid storeId, SearchTermsInput terms, RequestUser currentUser, CancellationToken token);
        Task<IEnumerable<ProducerSuggestDto>> SuggestProducersAsync(SearchTermsInput terms, RequestUser currentUser, CancellationToken token);
        IQueryable<ProducerSummaryDto> GetProducers(RequestUser currentUser);
    }
}