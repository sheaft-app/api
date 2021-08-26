using System;
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
using Sheaft.Mediatr.DeliveryBatch.Commands;

namespace Sheaft.GraphQL.DeliveryBatchs
{
    [ExtendObjectType(Name = "Mutation")]
    public class DeliveryBatchMutations : SheaftMutation
    {
        public DeliveryBatchMutations(
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            : base(currentUserService, httpContextAccessor)
        {
        }

        [GraphQLName("createDeliveryBatch")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(DeliveryBatchType))]
        public async Task<DeliveryBatch> CreateDeliveryBatchAsync(
            [GraphQLType(typeof(CreateDeliveryBatchInputType))] [GraphQLName("input")]
            CreateDeliveryBatchCommand input, [Service] ISheaftMediatr mediatr,
            DeliveryBatchesByIdBatchDataLoader deliveryBatchesDataLoader, CancellationToken token)
        {
            var result = await ExecuteAsync<CreateDeliveryBatchCommand, Guid>(mediatr, input, token);
            return await deliveryBatchesDataLoader.LoadAsync(result, token);
        }

        [GraphQLName("updateDeliveryBatch")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(DeliveryBatchType))]
        public async Task<DeliveryBatch> UpdateDeliveryBatchAsync(
            [GraphQLType(typeof(UpdateDeliveryBatchInputType))] [GraphQLName("input")]
            UpdateDeliveryBatchCommand input, [Service] ISheaftMediatr mediatr,
            DeliveryBatchesByIdBatchDataLoader deliveryBatchesDataLoader, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await deliveryBatchesDataLoader.LoadAsync(input.Id, token);
        }

        [GraphQLName("setNextDeliveryForBatch")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(DeliveryBatchType))]
        public async Task<DeliveryBatch> SetNextDeliveryAsync(
            [GraphQLType(typeof(SetNextDeliveryInputType))] [GraphQLName("input")]
            SetNextDeliveryCommand input, [Service] ISheaftMediatr mediatr,
            DeliveryBatchesByIdBatchDataLoader deliveryBatchesDataLoader, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await deliveryBatchesDataLoader.LoadAsync(input.DeliveryBatchId, token);
        }

        [GraphQLName("setDeliveryBatchAsReady")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(DeliveryBatchType))]
        public async Task<DeliveryBatch> SetDeliveryBatchAsReadyAsync(
            [GraphQLType(typeof(SetDeliveryBatchAsReadyInputType))] [GraphQLName("input")]
            SetDeliveryBatchAsReadyCommand input, [Service] ISheaftMediatr mediatr,
            DeliveryBatchesByIdBatchDataLoader deliveryBatchesDataLoader, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await deliveryBatchesDataLoader.LoadAsync(input.Id, token);
        }

        [GraphQLName("startDeliveryBatch")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(DeliveryBatchType))]
        public async Task<DeliveryBatch> StartDeliveryBatchAsync(
            [GraphQLType(typeof(StartDeliveryBatchInputType))] [GraphQLName("input")]
            StartDeliveryBatchCommand input, [Service] ISheaftMediatr mediatr,
            DeliveryBatchesByIdBatchDataLoader deliveryBatchesDataLoader, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await deliveryBatchesDataLoader.LoadAsync(input.DeliveryBatchId, token);
        }

        [GraphQLName("completeDeliveryBatch")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(DeliveryBatchType))]
        public async Task<DeliveryBatch> CompleteDeliveryBatchAsync(
            [GraphQLType(typeof(CompleteDeliveryBatchInputType))] [GraphQLName("input")]
            CompleteDeliveryBatchCommand input, [Service] ISheaftMediatr mediatr,
            DeliveryBatchesByIdBatchDataLoader deliveryBatchesDataLoader, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await deliveryBatchesDataLoader.LoadAsync(input.DeliveryBatchId, token);
        }

        [GraphQLName("postponeDeliveryBatch")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(DeliveryBatchType))]
        public async Task<DeliveryBatch> PostponeDeliveryBatchAsync(
            [GraphQLType(typeof(PostponeDeliveryBatchInputType))] [GraphQLName("input")]
            PostponeDeliveryBatchCommand input, [Service] ISheaftMediatr mediatr,
            DeliveryBatchesByIdBatchDataLoader deliveryBatchesDataLoader, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await deliveryBatchesDataLoader.LoadAsync(input.Id, token);
        }

        [GraphQLName("cancelDeliveryBatch")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(DeliveryBatchType))]
        public async Task<DeliveryBatch> CancelDeliveryBatchAsync(
            [GraphQLType(typeof(CancelDeliveryBatchInputType))] [GraphQLName("input")]
            CancelDeliveryBatchCommand input, [Service] ISheaftMediatr mediatr,
            DeliveryBatchesByIdBatchDataLoader deliveryBatchesDataLoader, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await deliveryBatchesDataLoader.LoadAsync(input.Id, token);
        }
    }
}