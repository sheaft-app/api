using System;
using System.Linq;
using System.Threading;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Application.Security;
using Sheaft.Core.Options;
using Sheaft.Domain;
using Sheaft.GraphQL.Types.Outputs;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.GraphQL.Observations
{
    [ExtendObjectType(Name = "Query")]
    public class ObservationQueries : SheaftQuery
    {
        private readonly RoleOptions _roleOptions;

        public ObservationQueries(
            IOptionsSnapshot<RoleOptions> roleOptions,
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            : base(currentUserService, httpContextAccessor)
        {
            _roleOptions = roleOptions.Value;
        }

        [GraphQLName("observation")]
        [GraphQLType(typeof(ObservationType))]
        [Authorize(Policy = Policies.REGISTERED)]
        [UseDbContext(typeof(QueryDbContext))]
        [UseSingleOrDefault]
        public IQueryable<Observation> GetObservation([ID] Guid id,
            [ScopedService] QueryDbContext context)
        {
            SetLogTransaction(id);

            return context.Observations
                .Where(c => c.Id == id);
        }

        [GraphQLName("observations")]
        [GraphQLType(typeof(ListType<ObservationType>))]
        [UseDbContext(typeof(QueryDbContext))]
        [Authorize(Policy = Policies.REGISTERED)]
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Observation> GetAll([ID] Guid? producerId, [ID] Guid? batchId, [ScopedService] QueryDbContext context,
            CancellationToken token)
        {
            SetLogTransaction();

            if (CurrentUser.IsInRole(_roleOptions.Producer.Value))
                return context.Observations
                    .Where(c => c.ProducerId == CurrentUser.Id && !c.ReplyToId.HasValue && (!batchId.HasValue || c.Batches.Any(b => b.BatchId == batchId.Value)));

            if (producerId.HasValue)
                return context.Observations
                    .Where(c => c.ProducerId == producerId && (c.VisibleToAll || c.UserId == CurrentUser.Id) && !c.ReplyToId.HasValue && (!batchId.HasValue || c.Batches.Any(b => b.BatchId == batchId.Value)));

            return context.Observations
                .Where(c => (c.VisibleToAll || c.UserId == CurrentUser.Id) && !c.ReplyToId.HasValue && (!batchId.HasValue || c.Batches.Any(b => b.BatchId == batchId.Value)));
        }
    }
}