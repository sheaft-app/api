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
using Sheaft.GraphQL.Types.Outputs;
using Sheaft.Mediatr.DeliveryClosing.Commands;
using Sheaft.Mediatr.DeliveryMode.Commands;

namespace Sheaft.GraphQL.DeliveryModes
{
    [ExtendObjectType(Name = "Mutation")]
    public class DeliveryModeMutations : SheaftMutation
    {
        public DeliveryModeMutations(
            ISheaftMediatr mediator,
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            : base(mediator, currentUserService, httpContextAccessor)
        {
        }
        
        [GraphQLName("createDeliveryMode")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(DeliveryModeType))]
        public async Task<DeliveryMode> CreateDeliveryModeAsync([GraphQLName("input")] CreateDeliveryModeCommand input,
            DeliveryModesByIdBatchDataLoader deliveriesDataLoader, CancellationToken token)
        {
            var result = await ExecuteAsync<CreateDeliveryModeCommand, Guid>(input, token);
            return await deliveriesDataLoader.LoadAsync(result, token);
        }
        
        [GraphQLName("updateDeliveryMode")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(DeliveryModeType))]
        public async Task<DeliveryMode> UpdateDeliveryModeAsync([GraphQLName("input")] UpdateDeliveryModeCommand input,
            DeliveryModesByIdBatchDataLoader deliveriesDataLoader, CancellationToken token)
        {
            await ExecuteAsync(input, token);
            return await deliveriesDataLoader.LoadAsync(input.DeliveryModeId, token);
        }
        
        [GraphQLName("setDeliveryModesAvailability")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(ListType<DeliveryModeType>))]
        public async Task<IEnumerable<DeliveryMode>> SetDeliveryModesAvailabilityAsync([GraphQLName("input")] SetDeliveryModesAvailabilityCommand input, 
            DeliveryModesByIdBatchDataLoader deliveriesDataLoader, CancellationToken token)
        {
            await ExecuteAsync(input, token);
            return await deliveriesDataLoader.LoadAsync(input.DeliveryModeIds.ToList(), token);
        }
        
        [GraphQLName("deleteDeliveryMode")]
        [Authorize(Policy = Policies.PRODUCER)]
        public async Task<bool> DeleteDeliveryModeAsync([GraphQLName("input")] DeleteDeliveryModeCommand input, CancellationToken token)
        {
            return await ExecuteAsync(input, token);
        }

        [GraphQLName("updateOrCreateDeliveryClosing")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(DeliveryClosingType))]
        public async Task<DeliveryClosing> UpdateOrCreateDeliveryClosingAsync(
            [GraphQLName("input")] UpdateOrCreateDeliveryClosingCommand input,
            DeliveryClosingsByIdBatchDataLoader businessClosingsDataLoader, CancellationToken token)
        {
            var result =
                await ExecuteAsync<UpdateOrCreateDeliveryClosingCommand, Guid>(input, token);
            return await businessClosingsDataLoader.LoadAsync(result, token);
        }

        [GraphQLName("updateOrCreateDeliveryClosings")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(ListType<DeliveryClosingType>))]
        public async Task<IEnumerable<DeliveryClosing>> UpdateOrCreateDeliveryClosingsAsync(
            [GraphQLName("input")] UpdateOrCreateDeliveryClosingsCommand input,
            DeliveryClosingsByIdBatchDataLoader businessClosingsDataLoader, CancellationToken token)
        {
            var result =
                await ExecuteAsync<UpdateOrCreateDeliveryClosingsCommand, IEnumerable<Guid>>(input, token);
            return await businessClosingsDataLoader.LoadAsync(result.ToList(), token);
        }

        [GraphQLName("deleteDeliveryClosings")]
        [Authorize(Policy = Policies.PRODUCER)]
        public async Task<bool> DeleteDeliveryClosingsAsync([GraphQLName("input")] DeleteDeliveryClosingsCommand input,
            CancellationToken token)
        {
            return await ExecuteAsync(input, token);
        }
    }
}