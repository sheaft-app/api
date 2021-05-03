using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Security;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.GraphQL.Types.Outputs;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.GraphQL.Jobs
{
    [ExtendObjectType(Name = "Query")]
    public class JobQueries : SheaftQuery
    {
        public JobQueries(
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            : base(currentUserService, httpContextAccessor)
        {
        }

        [GraphQLName("job")]
        [GraphQLType(typeof(JobType))]
        [UseDbContext(typeof(AppDbContext))]
        [Authorize(Policy = Policies.REGISTERED)]
        [UseSingleOrDefault]
        public IQueryable<Job> Get([ID] Guid id, [ScopedService] AppDbContext context)
        {
            SetLogTransaction(id);
            return context.Jobs
                .Where(c => c.Id == id && c.UserId == CurrentUser.Id && !c.Archived);
        }

        [GraphQLName("jobs")]
        [GraphQLType(typeof(ListType<JobType>))]
        [UseDbContext(typeof(AppDbContext))]
        [Authorize(Policy = Policies.REGISTERED)]
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Job> GetAll([ScopedService] AppDbContext context)
        {
            SetLogTransaction();
            return context.Jobs
                .Where(c => c.UserId == CurrentUser.Id && !c.Archived);
        }

        [GraphQLName("hasPendingJobs")]
        [GraphQLType(typeof(BooleanType))]
        [UseDbContext(typeof(AppDbContext))]
        [Authorize(Policy = Policies.REGISTERED)]
        public async Task<bool> Get(IEnumerable<JobKind> kinds, [ScopedService] AppDbContext context, CancellationToken token)
        {
            if (kinds == null)
                kinds = new List<JobKind>();

            SetLogTransaction();
            return await context.Jobs
                .AnyAsync(r => kinds.Contains(r.Kind) && !r.Archived &&
                            r.Kind == JobKind.ImportProducts &&
                            (r.Status == ProcessStatus.Paused || r.Status == ProcessStatus.Processing || r.Status == ProcessStatus.Waiting) &&
                            r.UserId == CurrentUser.Id, token);
        }
    }
}