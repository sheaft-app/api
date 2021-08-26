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
using Sheaft.Mediatr.BatchDefinition.Commands;

namespace Sheaft.GraphQL.BatchDefinitions
{
    [ExtendObjectType(Name = "Mutation")]
    public class BatchDefinitionMutations : SheaftMutation
    {
        public BatchDefinitionMutations(
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            : base(currentUserService, httpContextAccessor)
        {
        }

        [GraphQLName("createBatchDefinition")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(BatchDefinitionType))]
        public async Task<BatchDefinition> CreateBatchDefinitionAsync(
            [GraphQLType(typeof(CreateBatchDefinitionInputType))] [GraphQLName("input")]
            CreateBatchDefinitionCommand input, [Service] ISheaftMediatr mediatr,
            BatchDefinitionsByIdBatchDataLoader catalogsDataLoader, CancellationToken token)
        {
            var result = await ExecuteAsync<CreateBatchDefinitionCommand, Guid>(mediatr, input, token);
            return await catalogsDataLoader.LoadAsync(result, token);
        }

        [GraphQLName("updateBatchDefinition")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(BatchType))]
        public async Task<BatchDefinition> UpdateBatchDefinitionAsync(
            [GraphQLType(typeof(UpdateBatchDefinitionInputType))] [GraphQLName("input")]
            UpdateBatchDefinitionCommand input, [Service] ISheaftMediatr mediatr,
            BatchDefinitionsByIdBatchDataLoader catalogsDataLoader, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await catalogsDataLoader.LoadAsync(input.BatchDefinitionId, token);
        }

        [GraphQLName("deleteBatchDefinition")]
        [Authorize(Policy = Policies.PRODUCER)]
        public async Task<bool> DeleteBatchDefinitionAsync(
            [GraphQLType(typeof(DeleteBatchDefinitionInputType))] [GraphQLName("input")]
            DeleteBatchDefinitionCommand input, [Service] ISheaftMediatr mediatr,
            CancellationToken token)
        {
            return await ExecuteAsync(mediatr, input, token);
        }
    }
}