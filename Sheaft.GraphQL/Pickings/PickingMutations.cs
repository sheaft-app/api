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
using Sheaft.Mediatr.Picking.Commands;

namespace Sheaft.GraphQL.Pickings
{
    [ExtendObjectType(Name = "Mutation")]
    public class PickingMutations : SheaftMutation
    {
        public PickingMutations(
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            : base(currentUserService, httpContextAccessor)
        {
        }

        [GraphQLName("createPicking")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(PickingType))]
        public async Task<Picking> CreatePicking(
            [GraphQLType(typeof(CreatePickingInputType))] [GraphQLName("input")]
            CreatePickingCommand input, [Service] ISheaftMediatr mediatr,
            PickingsByIdBatchDataLoader dataLoader, CancellationToken token)
        {
            var result = await ExecuteAsync<CreatePickingCommand, Guid>(mediatr, input, token);
            return await dataLoader.LoadAsync(result, token);
        }

        [GraphQLName("updatePicking")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(PickingType))]
        public async Task<Picking> UpdatePicking(
            [GraphQLType(typeof(UpdatePickingInputType))] [GraphQLName("input")]
            UpdatePickingCommand input, [Service] ISheaftMediatr mediatr,
            PickingsByIdBatchDataLoader dataLoader, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await dataLoader.LoadAsync(input.PickingId, token);
        }

        [GraphQLName("startPicking")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(PickingType))]
        public async Task<Picking> StartPicking(
            [GraphQLType(typeof(StartPickingInputType))] [GraphQLName("input")]
            StartPickingCommand input, [Service] ISheaftMediatr mediatr,
            PickingsByIdBatchDataLoader dataLoader, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await dataLoader.LoadAsync(input.PickingId, token);
        }

        [GraphQLName("pausePicking")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(PickingType))]
        public async Task<Picking> PausePicking(
            [GraphQLType(typeof(PausePickingInputType))] [GraphQLName("input")]
            PausePickingCommand input, [Service] ISheaftMediatr mediatr,
            PickingsByIdBatchDataLoader dataLoader, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await dataLoader.LoadAsync(input.PickingId, token);
        }

        [GraphQLName("completePicking")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(PickingType))]
        public async Task<Picking> CompletePicking(
            [GraphQLType(typeof(CompletePickingInputType))] [GraphQLName("input")]
            CompletePickingCommand input, [Service] ISheaftMediatr mediatr,
            PickingsByIdBatchDataLoader dataLoader, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await dataLoader.LoadAsync(input.PickingId, token);
        }

        [GraphQLName("deletePicking")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(BooleanType))]
        public async Task<bool> DeletePicking(
            [GraphQLType(typeof(DeletePickingInputType))] [GraphQLName("input")]
            DeletePickingCommand input, [Service] ISheaftMediatr mediatr,
            CancellationToken token)
        {
            return await ExecuteAsync(mediatr, input, token);
        }

        [GraphQLName("setPickingProductPreparedQuantity")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(PickingType))]
        public async Task<Picking> SetPickingProductPreparedQuantity(
            [GraphQLType(typeof(SetPickingProductPreparedQuantityInputType))] [GraphQLName("input")]
            SetPickingProductPreparedQuantityCommand input, [Service] ISheaftMediatr mediatr,
            PickingsByIdBatchDataLoader dataLoader, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await dataLoader.LoadAsync(input.PickingId, token);
        }
    }
}