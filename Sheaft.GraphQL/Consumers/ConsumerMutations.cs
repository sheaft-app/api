using System;
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
using Sheaft.Mediatr.Consumer.Commands;
using Sheaft.Mediatr.Legal.Commands;

namespace Sheaft.GraphQL.Consumers
{
    [ExtendObjectType(Name = "Mutation")]
    public class ConsumerMutations : SheaftMutation
    {
        public ConsumerMutations(
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            : base(currentUserService, httpContextAccessor)
        {
        }

        [GraphQLName("registerConsumer")]
        [Authorize(Policy = Policies.UNREGISTERED)]
        [GraphQLType(typeof(ConsumerType))]
        public async Task<Consumer> RegisterConsumerAsync(
            [GraphQLType(typeof(RegisterConsumerInputType))] [GraphQLName("input")]
            RegisterConsumerCommand input, [Service] ISheaftMediatr mediatr,
            ConsumersByIdBatchDataLoader storesDataLoader, CancellationToken token)
        {
            var result = await ExecuteAsync<RegisterConsumerCommand, Guid>(mediatr, input, token);
            return await storesDataLoader.LoadAsync(result, token);
        }

        [GraphQLName("updateConsumer")]
        [Authorize(Policy = Policies.CONSUMER)]
        [GraphQLType(typeof(ConsumerType))]
        public async Task<Consumer> UpdateConsumerAsync(
            [GraphQLType(typeof(UpdateConsumerInputType))] [GraphQLName("input")]
            UpdateConsumerCommand input, [Service] ISheaftMediatr mediatr,
            ConsumersByIdBatchDataLoader storesDataLoader, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await storesDataLoader.LoadAsync(input.ConsumerId, token);
        }

        [GraphQLName("createConsumerLegals")]
        [Authorize(Policy = Policies.CONSUMER)]
        [GraphQLType(typeof(ConsumerLegalType))]
        public async Task<ConsumerLegal> CreateConsumerLegalsAsync(
            [GraphQLType(typeof(CreateConsumerLegalsInputType))] [GraphQLName("input")]
            CreateConsumerLegalCommand input, [Service] ISheaftMediatr mediatr,
            ConsumerLegalsByIdBatchDataLoader legalsDataLoader, CancellationToken token)
        {
            var result = await ExecuteAsync<CreateConsumerLegalCommand, Guid>(mediatr, input, token);
            return await legalsDataLoader.LoadAsync(result, token);
        }

        [GraphQLName("updateConsumerLegals")]
        [Authorize(Policy = Policies.CONSUMER)]
        [GraphQLType(typeof(ConsumerLegalType))]
        public async Task<ConsumerLegal> UpdateConsumerLegalsAsync(
            [GraphQLType(typeof(UpdateConsumerLegalsInputType))] [GraphQLName("input")]
            UpdateConsumerLegalCommand input, [Service] ISheaftMediatr mediatr,
            ConsumerLegalsByIdBatchDataLoader legalsDataLoader, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await legalsDataLoader.LoadAsync(input.LegalId, token);
        }
    }
}