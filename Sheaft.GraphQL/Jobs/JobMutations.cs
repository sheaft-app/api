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
using Sheaft.Mediatr.Job.Commands;

namespace Sheaft.GraphQL.Jobs
{
    [ExtendObjectType(Name = "Mutation")]
    public class JobMutations : SheaftMutation
    {
        public JobMutations(
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            : base(currentUserService, httpContextAccessor)
        {
        }

        [GraphQLName("resumeJobs")]
        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLType(typeof(ListType<JobType>))]
        public async Task<IEnumerable<Job>> ResumeJobsAsync(
            [GraphQLType(typeof(ResumeJobsInputType))] [GraphQLName("input")]
            ResumeJobsCommand input, [Service] ISheaftMediatr mediatr,
            JobsByIdBatchDataLoader jobsDataLoader, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await jobsDataLoader.LoadAsync(input.JobIds.ToList(), token);
        }

        [GraphQLName("pauseJobs")]
        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLType(typeof(ListType<JobType>))]
        public async Task<IEnumerable<Job>> PauseJobsAsync(
            [GraphQLType(typeof(PauseJobsInputType))] [GraphQLName("input")]
            PauseJobsCommand input, [Service] ISheaftMediatr mediatr,
            JobsByIdBatchDataLoader jobsDataLoader, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await jobsDataLoader.LoadAsync(input.JobIds.ToList(), token);
        }

        [GraphQLName("retryJobs")]
        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLType(typeof(ListType<JobType>))]
        public async Task<IEnumerable<Job>> RetryJobsAsync(
            [GraphQLType(typeof(RetryJobsInputType))] [GraphQLName("input")]
            RetryJobsCommand input, [Service] ISheaftMediatr mediatr,
            JobsByIdBatchDataLoader jobsDataLoader, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await jobsDataLoader.LoadAsync(input.JobIds.ToList(), token);
        }

        [GraphQLName("cancelJobs")]
        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLType(typeof(ListType<JobType>))]
        public async Task<IEnumerable<Job>> CancelJobsAsync(
            [GraphQLType(typeof(CancelJobsInputType))] [GraphQLName("input")]
            CancelJobsCommand input, [Service] ISheaftMediatr mediatr,
            JobsByIdBatchDataLoader jobsDataLoader, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await jobsDataLoader.LoadAsync(input.JobIds.ToList(), token);
        }

        [GraphQLName("archiveJobs")]
        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLType(typeof(ListType<JobType>))]
        public async Task<IEnumerable<Job>> ArchiveJobsAsync(
            [GraphQLType(typeof(ArchiveJobsInputType))] [GraphQLName("input")]
            ArchiveJobsCommand input, [Service] ISheaftMediatr mediatr,
            JobsByIdBatchDataLoader jobsDataLoader, CancellationToken token)
        {
            await ExecuteAsync(mediatr, input, token);
            return await jobsDataLoader.LoadAsync(input.JobIds.ToList(), token);
        }
    }
}