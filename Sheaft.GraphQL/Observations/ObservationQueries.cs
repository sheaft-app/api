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
using Microsoft.Extensions.Options;
using Sheaft.Application.Extensions;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Security;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.GraphQL.Types;
using Sheaft.GraphQL.Types.Outputs;
using Sheaft.Infrastructure.Persistence;
using Sheaft.Options;

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
        public IQueryable<Observation> GetAll([ID] Guid? producerId, [ScopedService] QueryDbContext context,
            CancellationToken token)
        {
            SetLogTransaction();

            if (CurrentUser.IsInRole(_roleOptions.Producer.Value))
                return context.Observations
                    .Where(c => c.ProducerId == CurrentUser.Id && !c.ReplyToId.HasValue);

            if (producerId.HasValue)
                return context.Observations
                    .Where(c => c.ProducerId == producerId && (c.VisibleToAll || c.UserId == CurrentUser.Id) && !c.ReplyToId.HasValue);

            return context.Observations
                .Where(c => (c.VisibleToAll || c.UserId == CurrentUser.Id) && !c.ReplyToId.HasValue);
        }
    }
}