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
using Sheaft.Mediatr.BatchObservation.Commands;
using Sheaft.Mediatr.Catalog.Commands;

namespace Sheaft.GraphQL.Catalogs
{
    [ExtendObjectType(Name = "Mutation")]
    public class BatchObservationMutations : SheaftMutation
    {
        public BatchObservationMutations(
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            : base(currentUserService, httpContextAccessor)
        {
        }

        [GraphQLName("createBatchObservation")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(BatchObservationType))]
        public async Task<BatchObservation> CreateBatchObservationAsync(
            [GraphQLType(typeof(CreateBatchObservationInputType))] [GraphQLName("input")]
            CreateBatchObservationCommand input, [Service] ISheaftMediatr mediatr,
            BatchObservationsByIdBatchDataLoader catalogsDataLoader, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await catalogsDataLoader.LoadAsync(input.BatchId, token);
        }

        [GraphQLName("updateBatchObservation")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(BatchType))]
        public async Task<BatchObservation> UpdateBatchObservationAsync(
            [GraphQLType(typeof(UpdateBatchObservationInputType))] [GraphQLName("input")]
            UpdateBatchObservationCommand input, [Service] ISheaftMediatr mediatr,
            BatchObservationsByIdBatchDataLoader catalogsDataLoader, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await catalogsDataLoader.LoadAsync(input.BatchObservationId, token);
        }

        [GraphQLName("deleteBatchObservation")]
        [Authorize(Policy = Policies.PRODUCER)]
        public async Task<bool> DeleteBatchObservationAsync(
            [GraphQLType(typeof(DeleteBatchObservationInputType))] [GraphQLName("input")]
            DeleteBatchObservationCommand input, [Service] ISheaftMediatr mediatr,
            CancellationToken token)
        {
            return await ExecuteAsync(mediatr, input, token);
        }
    }
}