using System;
using System.Linq;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Microsoft.AspNetCore.Http;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Security;
using Sheaft.Domain;
using Sheaft.GraphQL.Types.Outputs;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.GraphQL.PreAuthorizations
{
    [ExtendObjectType(Name = "Query")]
    public class PreAuthorizationQueries : SheaftQuery
    {
        public PreAuthorizationQueries(
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            :base(currentUserService, httpContextAccessor)
        {
        }

        [GraphQLName("preAuthorization")]
        [GraphQLType(typeof(PreAuthorizationType))]
        [UseDbContext(typeof(QueryDbContext))]
        [Authorize(Policy = Policies.CONSUMER)]
        [UseSingleOrDefault]
        public IQueryable<PreAuthorization> Get([ID] Guid id, [ScopedService] QueryDbContext context)
        {
            SetLogTransaction(id);
            return context.PreAuthorizations
                .Where(d => d.Id == id && d.Order.UserId == CurrentUser.Id);
        }
    }
}