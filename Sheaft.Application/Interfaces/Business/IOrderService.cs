using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Application.Models;
using Sheaft.Core;
using Sheaft.Domain;

namespace Sheaft.Application.Interfaces.Business
{
    public interface IOrderService
    {
        Task<Result> ValidateConsumerOrderAsync(Guid orderId, RequestUser requestUser, CancellationToken token);

        Task<Result<List<Tuple<Domain.DeliveryMode, DateTimeOffset, string>>>> GetCartDeliveriesAsync(
            IEnumerable<ProducerExpectedDeliveryInputDto> producersExpectedDeliveries,
            IEnumerable<Tuple<Domain.Product, Guid, int>> cartProducts, CancellationToken token);

        Task<Result<List<Tuple<Domain.Product, Guid, int>>>> GetCartProductsAsync(
            IEnumerable<ResourceIdQuantityInputDto> productsQuantities, CancellationToken token);
    }
}