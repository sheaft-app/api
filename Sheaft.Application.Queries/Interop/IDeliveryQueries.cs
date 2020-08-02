using Sheaft.Interop;
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
        IQueryable<DeliveryModeDto> GetDelivery(Guid id, IRequestUser currentUser);
        IQueryable<DeliveryModeDto> GetDeliveries(IRequestUser currentUser);
        Task<IEnumerable<ProducerDeliveriesDto>> GetProducersDeliveriesAsync(IEnumerable<Guid> producerIds, IEnumerable<DeliveryKind> kinds, DateTimeOffset currentDate, IRequestUser currentUser, CancellationToken token);
        Task<IEnumerable<ProducerDeliveriesDto>> GetStoreDeliveriesForProducersAsync(Guid storeId, IEnumerable<Guid> producerIds, IEnumerable<DeliveryKind> kinds, DateTimeOffset currentDate, IRequestUser currentUser, CancellationToken token);
    }
}