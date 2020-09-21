using Sheaft.Application.Models;
using Sheaft.Core;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Application.Queries
{
    public interface IBusinessQueries
    {
        IQueryable<ProducerDto> GetProducer(Guid id, RequestUser currentUser);
        IQueryable<StoreDto> GetStore(Guid id, RequestUser currentUser);
        IQueryable<BusinessProfileDto> GetMyProfile(RequestUser currentUser);
        Task<SirenBusinessDto> RetrieveSiretInfosAsync(string siret, RequestUser currentUser, CancellationToken token);
        Task<ProducersSearchDto> SearchProducersAsync(Guid storeId, SearchTermsInput terms, RequestUser currentUser, CancellationToken token);
        Task<StoresSearchDto> SearchStoresAsync(Guid producerId, SearchTermsInput terms, RequestUser currentUser, CancellationToken token);
    }
}