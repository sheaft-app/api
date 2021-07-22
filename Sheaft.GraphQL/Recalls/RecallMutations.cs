using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Security;
using Sheaft.Domain;
using Sheaft.GraphQL.Jobs;
using Sheaft.GraphQL.Types.Inputs;
using Sheaft.GraphQL.Types.Outputs;
using Sheaft.Mediatr.Recall.Commands;

namespace Sheaft.GraphQL.Recalls
{
    [ExtendObjectType(Name = "Mutation")]
    public class RecallMutations : SheaftMutation
    {
        public RecallMutations(
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            : base(currentUserService, httpContextAccessor)
        {
        }

        [GraphQLName("createRecall")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(RecallType))]
        public async Task<Recall> CreateRecallAsync(
            [GraphQLType(typeof(CreateRecallInputType))] [GraphQLName("input")]
            CreateRecallCommand input, [Service] ISheaftMediatr mediatr,
            RecallsByIdBatchDataLoader dataLoader, CancellationToken token)
        {
            var result = await ExecuteAsync<CreateRecallCommand, Guid>(mediatr, input, token);
            return await dataLoader.LoadAsync(result, token);
        }

        [GraphQLName("updateRecall")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(RecallType))]
        public async Task<Recall> UpdateRecallAsync(
            [GraphQLType(typeof(UpdateRecallInputType))] [GraphQLName("input")]
            UpdateRecallCommand input, [Service] ISheaftMediatr mediatr,
            RecallsByIdBatchDataLoader dataLoader, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await dataLoader.LoadAsync(input.RecallId, token);
        }

        [GraphQLName("sendRecalls")]
        [Authorize(Policy = Policies.PRODUCER)]
        [GraphQLType(typeof(ListType<JobType>))]
        public async Task<IEnumerable<Job>> QueueSendRecallsAsync(
            [GraphQLType(typeof(QueueSendRecallsInputType))] [GraphQLName("input")]
            QueueSendRecallsCommand input, [Service] ISheaftMediatr mediatr,
            JobsByIdBatchDataLoader dataLoader, CancellationToken token)
        {
            var result = await ExecuteAsync<QueueSendRecallsCommand, IEnumerable<Guid>>(mediatr, input, token);
            return await dataLoader.LoadAsync(result.ToList(), token);
        }

        [GraphQLName("deleteRecalls")]
        [Authorize(Policy = Policies.PRODUCER)]
        public async Task<bool> DeleteRecallsAsync(
            [GraphQLType(typeof(DeleteRecallsInputType))] [GraphQLName("input")]
            DeleteRecallsCommand input, [Service] ISheaftMediatr mediatr,
            CancellationToken token)
        {
            return await ExecuteAsync(mediatr, input, token);
        }
    }
}