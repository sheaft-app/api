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

namespace Sheaft.GraphQL.Donations
{
    [ExtendObjectType(Name = "Query")]
    public class DonationsQueries : SheaftQuery
    {
        public DonationsQueries(
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            :base(currentUserService, httpContextAccessor)
        {
        }

        [GraphQLName("donation")]
        [GraphQLType(typeof(DonationType))]
        [UseDbContext(typeof(QueryDbContext))]
        [Authorize(Policy = Policies.REGISTERED)]
        [UseSingleOrDefault]
        public IQueryable<Donation> Get([ID] Guid id, [ScopedService] QueryDbContext context)
        {
            SetLogTransaction(id);
            return context.Donations
                .Where(d => d.Id == id && d.AuthorId == CurrentUser.Id);
        }
        
        [GraphQLName("donations")]
        [GraphQLType(typeof(ListType<DonationType>))]
        [UseDbContext(typeof(QueryDbContext))]
        [Authorize(Policy = Policies.REGISTERED)]
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Donation> GetAll([ScopedService] QueryDbContext context)
        {
            SetLogTransaction();
            return context.Donations
                .Where(d => d.AuthorId == CurrentUser.Id);
        }
    }
}