using Sheaft.Core;
using Sheaft.Interop.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Models.Dto;

namespace Sheaft.Application.Queries
{
    public interface IDeliveryQueries
    {
        IQueryable<DeliveryModeDto> GetDelivery(Guid id, RequestUser currentUser);
        IQueryable<DeliveryModeDto> GetDeliveries(RequestUser currentUser);
        Task<IEnumerable<ProducerDeliveriesDto>> GetProducersDeliveriesAsync(IEnumerable<Guid> producerIds, IEnumerable<DeliveryKind> kinds, DateTimeOffset currentDate, RequestUser currentUser, CancellationToken token);
        Task<IEnumerable<ProducerDeliveriesDto>> GetStoreDeliveriesForProducersAsync(Guid storeId, IEnumerable<Guid> producerIds, IEnumerable<DeliveryKind> kinds, DateTimeOffset currentDate, RequestUser currentUser, CancellationToken token);
    }
}