using System;
using System.Linq;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Microsoft.AspNetCore.Http;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Application.Security;
using Sheaft.Domain;
using Sheaft.GraphQL.Types.Outputs;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.GraphQL.Payouts
{
    [ExtendObjectType(Name = "Query")]
    public class PayoutQueries : SheaftQuery
    {
        public PayoutQueries(
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            :base(currentUserService, httpContextAccessor)
        {
        }

        [GraphQLName("payout")]
        [GraphQLType(typeof(PayoutType))]
        [UseDbContext(typeof(QueryDbContext))]
        [Authorize(Policy = Policies.STORE_OR_PRODUCER)]
        [UseSingleOrDefault]
        public IQueryable<Payout> Get([ID] Guid id, [ScopedService] QueryDbContext context)
        {
            SetLogTransaction(id);
            return context.Payouts
                .Where(d => d.Id == id && d.AuthorId == CurrentUser.Id);
        }
        
        [GraphQLName("payouts")]
        [GraphQLType(typeof(ListType<PayoutType>))]
        [UseDbContext(typeof(QueryDbContext))]
        [Authorize(Policy = Policies.STORE_OR_PRODUCER)]
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Payout> GetAll([ScopedService] QueryDbContext context)
        {
            SetLogTransaction();
            return context.Payouts
                .Where(d => d.AuthorId == CurrentUser.Id);
        }
    }
}