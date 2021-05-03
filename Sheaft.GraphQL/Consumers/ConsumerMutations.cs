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
using Sheaft.GraphQL.Types.Outputs;
using Sheaft.Mediatr.Consumer.Commands;
using Sheaft.Mediatr.Legal.Commands;

namespace Sheaft.GraphQL.Consumers
{
    [ExtendObjectType(Name = "Mutation")]
    public class ConsumerMutations : SheaftMutation
    {
        public ConsumerMutations(
            ISheaftMediatr mediator,
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            : base(mediator, currentUserService, httpContextAccessor)
        {
        }
        
        [GraphQLName("registerConsumer")]
        [Authorize(Policy = Policies.UNREGISTERED)]
        [GraphQLType(typeof(ConsumerType))]
        public async Task<Consumer> RegisterConsumerAsync([GraphQLName("input")] RegisterConsumerCommand input,
            ConsumersByIdBatchDataLoader storesDataLoader, CancellationToken token)
        {
            var result = await ExecuteAsync<RegisterConsumerCommand, Guid>(input, token);
            return await storesDataLoader.LoadAsync(result, token);
        }
        
        [GraphQLName("updateConsumer")]
        [Authorize(Policy = Policies.CONSUMER)]
        [GraphQLType(typeof(ConsumerType))]
        public async Task<Consumer> UpdateConsumerAsync([GraphQLName("input")] UpdateConsumerCommand input,
            ConsumersByIdBatchDataLoader storesDataLoader, CancellationToken token)
        {
            await ExecuteAsync(input, token);
            return await storesDataLoader.LoadAsync(input.ConsumerId, token);
        }

        [GraphQLName("createConsumerLegals")]
        [Authorize(Policy = Policies.CONSUMER)]
        [GraphQLType(typeof(ConsumerLegalType))]
        public async Task<ConsumerLegal> CreateConsumerLegalsAsync([GraphQLName("input")] CreateConsumerLegalCommand input,
            ConsumerLegalsByIdBatchDataLoader legalsDataLoader, CancellationToken token)
        {
            var result = await ExecuteAsync<CreateConsumerLegalCommand, Guid>(input, token);
            return await legalsDataLoader.LoadAsync(result, token);
        }
        
        [GraphQLName("updateConsumerLegals")]
        [Authorize(Policy = Policies.CONSUMER)]
        [GraphQLType(typeof(ConsumerLegalType))]
        public async Task<ConsumerLegal> UpdateConsumerLegalsAsync([GraphQLName("input")] UpdateConsumerLegalCommand input,
            ConsumerLegalsByIdBatchDataLoader legalsDataLoader, CancellationToken token)
        {
            await ExecuteAsync(input, token);
            return await legalsDataLoader.LoadAsync(input.LegalId, token);
        }
    }
}