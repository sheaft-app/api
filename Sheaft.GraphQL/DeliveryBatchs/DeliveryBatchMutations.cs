using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Microsoft.AspNetCore.Http;
using Sheaft.Application.Interfaces.Business;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Models;
using Sheaft.Application.Security;
using Sheaft.Domain;
using Sheaft.GraphQL.Types.Inputs;
using Sheaft.GraphQL.Types.Outputs;
using Sheaft.Infrastructure.Persistence;
using Sheaft.Mediatr.DeliveryBatch.Commands;
using Sheaft.Mediatr.DeliveryMode.Commands;

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
        public async Task<DeliveryBatch> CreateDeliveryModeAsync(
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
        public async Task<DeliveryBatch> UpdateDeliveryModeAsync(
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

        [GraphQLName("startDeliveryBatch")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(DeliveryBatchType))]
        public async Task<DeliveryBatch> StartDeliveryModeAsync(
            [GraphQLType(typeof(StartDeliveryBatchInputType))] [GraphQLName("input")]
            StartDeliveryBatchCommand input, [Service] ISheaftMediatr mediatr,
            DeliveryBatchesByIdBatchDataLoader deliveryBatchesDataLoader, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await deliveryBatchesDataLoader.LoadAsync(input.Id, token);
        }

        [GraphQLName("completeDeliveryBatch")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(DeliveryBatchType))]
        public async Task<DeliveryBatch> CompleteDeliveryModeAsync(
            [GraphQLType(typeof(CompleteDeliveryBatchInputType))] [GraphQLName("input")]
            CompleteDeliveryBatchCommand input, [Service] ISheaftMediatr mediatr,
            DeliveryBatchesByIdBatchDataLoader deliveryBatchesDataLoader, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await deliveryBatchesDataLoader.LoadAsync(input.Id, token);
        }

        [GraphQLName("postponeDeliveryBatch")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(DeliveryBatchType))]
        public async Task<DeliveryBatch> PostponeDeliveryModeAsync(
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
        public async Task<DeliveryBatch> CancelDeliveryModeAsync(
            [GraphQLType(typeof(CancelDeliveryBatchInputType))] [GraphQLName("input")]
            CancelDeliveryBatchCommand input, [Service] ISheaftMediatr mediatr,
            DeliveryBatchesByIdBatchDataLoader deliveryBatchesDataLoader, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await deliveryBatchesDataLoader.LoadAsync(input.Id, token);
        }

        [GraphQLName("deleteDeliveryBatch")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(BooleanType))]
        public async Task<bool> DeleteDeliveryModeAsync(
            [GraphQLType(typeof(DeleteDeliveryBatchInputType))] [GraphQLName("input")]
            DeleteDeliveryBatchCommand input, [Service] ISheaftMediatr mediatr, CancellationToken token)
        {
            return await ExecuteAsync(mediatr, input, token);
        }
    }
}