using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Domain;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Common.Interfaces.Queries
{
    public interface IDeliveryQueries
    {
        IQueryable<DeliveryModeDto> GetDelivery(Guid id, RequestUser currentUser);
        IQueryable<DeliveryModeDto> GetDeliveries(RequestUser currentUser);
        Task<IEnumerable<ProducerDeliveriesDto>> GetProducersDeliveriesAsync(IEnumerable<Guid> producerIds, IEnumerable<DeliveryKind> kinds, DateTimeOffset currentDate, RequestUser currentUser, CancellationToken token);
        Task<IEnumerable<ProducerDeliveriesDto>> GetStoreDeliveriesForProducersAsync(Guid storeId, IEnumerable<Guid> producerIds, IEnumerable<DeliveryKind> kinds, DateTimeOffset currentDate, RequestUser currentUser, CancellationToken token);
        Task<IEnumerable<ProducerDeliveriesDto>> GetNotCapedProducersDeliveriesAsync(IEnumerable<ProducerDeliveriesDto> producersDeliveries, CancellationToken token);
    }
}