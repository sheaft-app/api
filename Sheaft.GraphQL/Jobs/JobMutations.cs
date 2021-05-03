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
using Sheaft.Mediatr.Job.Commands;

namespace Sheaft.GraphQL.Jobs
{
    [ExtendObjectType(Name = "Mutation")]
    public class JobMutations : SheaftMutation
    {
        public JobMutations(
            ISheaftMediatr mediator,
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            : base(mediator, currentUserService, httpContextAccessor)
        {
        }

        [GraphQLName("resumeJobs")]
        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLType(typeof(ListType<JobType>))]
        public async Task<IEnumerable<Job>> ResumeJobsAsync([GraphQLName("input")] ResumeJobsCommand input,
            JobsByIdBatchDataLoader jobsDataLoader, CancellationToken token)
        {
            await ExecuteAsync(input, token);
            return await jobsDataLoader.LoadAsync(input.JobIds.ToList(), token);
        }

        [GraphQLName("pauseJobs")]
        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLType(typeof(ListType<JobType>))]
        public async Task<IEnumerable<Job>> PauseJobsAsync([GraphQLName("input")] PauseJobsCommand input,
            JobsByIdBatchDataLoader jobsDataLoader, CancellationToken token)
        {
            await ExecuteAsync(input, token);
            return await jobsDataLoader.LoadAsync(input.JobIds.ToList(), token);
        }

        [GraphQLName("retryJobs")]
        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLType(typeof(ListType<JobType>))]
        public async Task<IEnumerable<Job>> RetryJobsAsync([GraphQLName("input")] RetryJobsCommand input,
            JobsByIdBatchDataLoader jobsDataLoader, CancellationToken token)
        {
            await ExecuteAsync(input, token);
            return await jobsDataLoader.LoadAsync(input.JobIds.ToList(), token);
        }

        [GraphQLName("cancelJobs")]
        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLType(typeof(ListType<JobType>))]
        public async Task<IEnumerable<Job>> CancelJobsAsync([GraphQLName("input")] CancelJobsCommand input,
            JobsByIdBatchDataLoader jobsDataLoader, CancellationToken token)
        {
            await ExecuteAsync(input, token);
            return await jobsDataLoader.LoadAsync(input.JobIds.ToList(), token);
        }

        [GraphQLName("archiveJobs")]
        [Authorize(Policy = Policies.REGISTERED)]
        [GraphQLType(typeof(ListType<JobType>))]
        public async Task<IEnumerable<Job>> ArchiveJobsAsync([GraphQLName("input")] ArchiveJobsCommand input,
            JobsByIdBatchDataLoader jobsDataLoader, CancellationToken token)
        {
            await ExecuteAsync(input, token);
            return await jobsDataLoader.LoadAsync(input.JobIds.ToList(), token);
        }
    }
}