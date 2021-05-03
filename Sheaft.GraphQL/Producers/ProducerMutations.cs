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
using Sheaft.Mediatr.Producer.Commands;

namespace Sheaft.GraphQL.Producers
{
    [ExtendObjectType(Name = "Mutation")]
    public class ProducerMutations : SheaftMutation
    {
        public ProducerMutations(
            ISheaftMediatr mediator,
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            : base(mediator, currentUserService, httpContextAccessor)
        {
        }
        
        [GraphQLName("registerProducer")]
        [Authorize(Policy = Policies.UNREGISTERED)]
        [GraphQLType(typeof(ProducerType))]
        public async Task<Producer> RegisterProducerAsync([GraphQLName("input")] RegisterProducerCommand input,
            ProducersByIdBatchDataLoader producersDataLoader, CancellationToken token)
        {
            var result = await ExecuteAsync<RegisterProducerCommand, Guid>(input, token);
            return await producersDataLoader.LoadAsync(result, token);
        }
        
        [GraphQLName("updateProducer")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(ProducerType))]
        public async Task<Producer> UpdateProducerAsync([GraphQLName("input")] UpdateProducerCommand input,
            ProducersByIdBatchDataLoader producersDataLoader, CancellationToken token)
        {
            await ExecuteAsync(input, token);
            return await producersDataLoader.LoadAsync(input.ProducerId, token);
        }
    }
}