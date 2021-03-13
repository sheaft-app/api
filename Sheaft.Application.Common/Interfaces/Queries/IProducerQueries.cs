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
        IQueryable<ProducerDto> GetProducer(Guid id, RequestUser currentUser);
        IQueryable<ProducerDto> GetProducers(RequestUser currentUser);
        Task<ProducersSearchDto> SearchProducersAsync(Guid storeId, SearchTermsInput terms, RequestUser currentUser, CancellationToken token);
        Task<IEnumerable<SuggestProducerDto>> SuggestProducersAsync(SearchTermsInput terms, RequestUser currentUser, CancellationToken token);
    }
}