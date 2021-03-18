using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Interfaces.Queries
{
    public interface IProducerQueries
    {
        IQueryable<ProducerDto> GetProducer(Guid id, RequestUser currentUser);
        IQueryable<ProducerDto> GetProducers(RequestUser currentUser);
        Task<ProducersSearchDto> SearchProducersAsync(Guid storeId, SearchTermsDto terms, RequestUser currentUser, CancellationToken token);
        Task<IEnumerable<SuggestProducerDto>> SuggestProducersAsync(SearchTermsDto terms, RequestUser currentUser, CancellationToken token);
    }
}