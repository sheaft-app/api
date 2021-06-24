using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Security;
using Sheaft.Domain;
using Sheaft.GraphQL.Types.Inputs;
using Sheaft.GraphQL.Types.Outputs;
using Sheaft.Mediatr.DeliveryMode.Commands;

namespace Sheaft.GraphQL.DeliveryModes
{
    [ExtendObjectType(Name = "Mutation")]
    public class DeliveryModeMutations : SheaftMutation
    {
        public DeliveryModeMutations(
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            : base(currentUserService, httpContextAccessor)
        {
        }

        [GraphQLName("createDeliveryMode")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(DeliveryModeType))]
        public async Task<DeliveryMode> CreateDeliveryModeAsync(
            [GraphQLType(typeof(CreateDeliveryModeInputType))] [GraphQLName("input")]
            CreateDeliveryModeCommand input, [Service] ISheaftMediatr mediatr,
            DeliveryModesByIdBatchDataLoader deliveriesDataLoader, CancellationToken token)
        {
            var result = await ExecuteAsync<CreateDeliveryModeCommand, Guid>(mediatr, input, token);
            return await deliveriesDataLoader.LoadAsync(result, token);
        }

        [GraphQLName("updateDeliveryMode")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(DeliveryModeType))]
        public async Task<DeliveryMode> UpdateDeliveryModeAsync(
            [GraphQLType(typeof(UpdateDeliveryModeInputType))] [GraphQLName("input")]
            UpdateDeliveryModeCommand input, [Service] ISheaftMediatr mediatr,
            DeliveryModesByIdBatchDataLoader deliveriesDataLoader, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await deliveriesDataLoader.LoadAsync(input.DeliveryModeId, token);
        }

        [GraphQLName("setDeliveryModesAvailability")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(ListType<DeliveryModeType>))]
        public async Task<IEnumerable<DeliveryMode>> SetDeliveryModesAvailabilityAsync(
            [GraphQLType(typeof(SetDeliveryModesAvailabilityInputType))] [GraphQLName("input")]
            SetDeliveryModesAvailabilityCommand input, [Service] ISheaftMediatr mediatr,
            DeliveryModesByIdBatchDataLoader deliveriesDataLoader, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await deliveriesDataLoader.LoadAsync(input.DeliveryModeIds.ToList(), token);
        }

        [GraphQLName("deleteDeliveryMode")]
        [Authorize(Policy = Policies.PRODUCER)]
        public async Task<bool> DeleteDeliveryModeAsync(
            [GraphQLType(typeof(DeleteDeliveryModeInputType))] [GraphQLName("input")]
            DeleteDeliveryModeCommand input, [Service] ISheaftMediatr mediatr, CancellationToken token)
        {
            return await ExecuteAsync(mediatr, input, token);
        }

        [GraphQLName("updateOrCreateDeliveryModeClosing")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(DeliveryClosingType))]
        public async Task<DeliveryClosing> UpdateOrCreateDeliveryClosingAsync(
            [GraphQLType(typeof(UpdateOrCreateDeliveryClosingInputType))] [GraphQLName("input")]
            UpdateOrCreateDeliveryClosingCommand input, [Service] ISheaftMediatr mediatr,
            DeliveryClosingsByIdBatchDataLoader businessClosingsDataLoader, CancellationToken token)
        {
            var result =
                await ExecuteAsync<UpdateOrCreateDeliveryClosingCommand, Guid>(mediatr, input, token);
            return await businessClosingsDataLoader.LoadAsync(result, token);
        }

        [GraphQLName("updateOrCreateDeliveryModeClosings")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(ListType<DeliveryClosingType>))]
        public async Task<IEnumerable<DeliveryClosing>> UpdateOrCreateDeliveryClosingsAsync(
            [GraphQLType(typeof(UpdateOrCreateDeliveryClosingsInputType))] [GraphQLName("input")]
            UpdateOrCreateDeliveryClosingsCommand input, [Service] ISheaftMediatr mediatr,
            DeliveryClosingsByIdBatchDataLoader businessClosingsDataLoader, CancellationToken token)
        {
            var result =
                await ExecuteAsync<UpdateOrCreateDeliveryClosingsCommand, IEnumerable<Guid>>(mediatr, input, token);
            return await businessClosingsDataLoader.LoadAsync(result.ToList(), token);
        }

        [GraphQLName("deleteDeliveryModeClosings")]
        [Authorize(Policy = Policies.PRODUCER)]
        public async Task<bool> DeleteDeliveryClosingsAsync(
            [GraphQLType(typeof(DeleteDeliveryClosingsInputType))] [GraphQLName("input")]
            DeleteDeliveryClosingsCommand input, [Service] ISheaftMediatr mediatr,
            CancellationToken token)
        {
            return await ExecuteAsync(mediatr, input, token);
        }
    }
}