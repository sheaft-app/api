using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Application.Security;
using Sheaft.Domain;
using Sheaft.GraphQL.Types.Inputs;
using Sheaft.GraphQL.Types.Outputs;
using Sheaft.Mediatr.Billing.Commands;
using Sheaft.Mediatr.Delivery.Commands;

namespace Sheaft.GraphQL.Deliveries
{
    [ExtendObjectType(Name = "Mutation")]
    public class DeliveryMutations : SheaftMutation
    {
        public DeliveryMutations(
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            : base(currentUserService, httpContextAccessor)
        {
        }

        [GraphQLName("startDelivery")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(DeliveryType))]
        public async Task<Delivery> StartDelivery(
            [GraphQLType(typeof(StartDeliveryInputType))] [GraphQLName("input")]
            StartDeliveryCommand input, [Service] ISheaftMediatr mediatr,
            DeliveriesByIdBatchDataLoader dataLoader, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await dataLoader.LoadAsync(input.DeliveryId, token);
        }

        [GraphQLName("completeDelivery")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(DeliveryType))]
        public async Task<Delivery> CompleteDelivery(
            [GraphQLType(typeof(CompleteDeliveryInputType))] [GraphQLName("input")]
            CompleteDeliveryCommand input, [Service] ISheaftMediatr mediatr,
            DeliveriesByIdBatchDataLoader dataLoader, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await dataLoader.LoadAsync(input.DeliveryId, token);
        }

        [GraphQLName("skipDelivery")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(DeliveryType))]
        public async Task<Delivery> SkipDelivery(
            [GraphQLType(typeof(SkipDeliveryInputType))] [GraphQLName("input")]
            SkipDeliveryCommand input, [Service] ISheaftMediatr mediatr,
            DeliveriesByIdBatchDataLoader dataLoader, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await dataLoader.LoadAsync(input.DeliveryId, token);
        }

        [GraphQLName("rejectDelivery")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(DeliveryType))]
        public async Task<Delivery> RejectDelivery(
            [GraphQLType(typeof(RejectDeliveryInputType))] [GraphQLName("input")]
            RejectDeliveryCommand input, [Service] ISheaftMediatr mediatr,
            DeliveriesByIdBatchDataLoader dataLoader, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await dataLoader.LoadAsync(input.DeliveryId, token);
        }

        [GraphQLName("markDeliveriesAsBilled")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(ListType<DeliveryType>))]
        public async Task<IEnumerable<Delivery>> MarkDeliveriesAsBilled(
            [GraphQLType(typeof(MarkDeliveriesAsBilledInputType))] [GraphQLName("input")]
            MarkDeliveriesAsBilledCommand input, [Service] ISheaftMediatr mediatr,
            DeliveriesByIdBatchDataLoader dataLoader, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await dataLoader.LoadAsync(input.DeliveryIds.ToList(), token);
        }

        [GraphQLName("markTimeRangeDeliveriesAsBilled")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(ListType<DeliveryType>))]
        public async Task<IEnumerable<Delivery>> MarkTimeRangeDeliveriesAsBilled(
            [GraphQLType(typeof(MarkTimeRangeDeliveriesAsBilledInputType))] [GraphQLName("input")]
            MarkTimeRangeDeliveriesAsBilledCommand input, [Service] ISheaftMediatr mediatr,
            DeliveriesByIdBatchDataLoader dataLoader, CancellationToken token)
        {
            var result = await ExecuteAsync<MarkTimeRangeDeliveriesAsBilledCommand, IEnumerable<Guid>>(mediatr, input, token);
            return await dataLoader.LoadAsync(result.ToList(), token);
        }
    }
}