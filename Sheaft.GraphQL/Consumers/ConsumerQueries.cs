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

namespace Sheaft.GraphQL.Consumers
{
    [ExtendObjectType(Name = "Query")]
    public class ConsumerQueries : SheaftQuery
    {
        public ConsumerQueries(
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            :base(currentUserService, httpContextAccessor)
        {
        }

        [GraphQLName("consumerLegals")]
        [GraphQLType(typeof(ConsumerLegalType))]
        [UseDbContext(typeof(QueryDbContext))]
        [Authorize(Policy = Policies.CONSUMER)]
        [UseSingleOrDefault]
        public IQueryable<ConsumerLegal> GetLegals([ScopedService] QueryDbContext context)
        {
            SetLogTransaction();
            return context.Set<ConsumerLegal>()
                .Where(d => d.UserId == CurrentUser.Id);
        }
    }
}