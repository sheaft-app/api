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
using Sheaft.GraphQL.Batches;
using Sheaft.GraphQL.Types.Inputs;
using Sheaft.GraphQL.Types.Outputs;
using Sheaft.Mediatr.Batch.Commands;
using Sheaft.Mediatr.Catalog.Commands;

namespace Sheaft.GraphQL.Catalogs
{
    [ExtendObjectType(Name = "Mutation")]
    public class BatchMutations : SheaftMutation
    {
        public BatchMutations(
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            : base(currentUserService, httpContextAccessor)
        {
        }

        [GraphQLName("createBatch")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(BatchType))]
        public async Task<Batch> CreateBatchAsync(
            [GraphQLType(typeof(CreateBatchInputType))] [GraphQLName("input")]
            CreateBatchCommand input, [Service] ISheaftMediatr mediatr,
            BatchesByIdBatchDataLoader catalogsDataLoader, CancellationToken token)
        {
            var result = await ExecuteAsync<CreateBatchCommand, Guid>(mediatr, input, token);
            return await catalogsDataLoader.LoadAsync(result, token);
        }

        [GraphQLName("updateBatch")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(BatchType))]
        public async Task<Batch> UpdateBatchAsync(
            [GraphQLType(typeof(UpdateBatchInputType))] [GraphQLName("input")]
            UpdateBatchCommand input, [Service] ISheaftMediatr mediatr,
            BatchesByIdBatchDataLoader catalogsDataLoader, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await catalogsDataLoader.LoadAsync(input.BatchId, token);
        }

        [GraphQLName("deleteBatch")]
        [Authorize(Policy = Policies.PRODUCER)]
        public async Task<bool> DeleteBatchAsync(
            [GraphQLType(typeof(DeleteBatchInputType))] [GraphQLName("input")]
            DeleteBatchCommand input, [Service] ISheaftMediatr mediatr,
            CancellationToken token)
        {
            return await ExecuteAsync(mediatr, input, token);
        }
    }
}