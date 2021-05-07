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

namespace Sheaft.GraphQL.QuickOrders
{
    [ExtendObjectType(Name = "Query")]
    public class QuickOrderQueries : SheaftQuery
    {
        public QuickOrderQueries(
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            :base(currentUserService, httpContextAccessor)
        {
        }

        [GraphQLName("quickOrder")]
        [GraphQLType(typeof(QuickOrderType))]
        [UseDbContext(typeof(QueryDbContext))]
        [Authorize(Policy = Policies.STORE)]
        [UseSingleOrDefault]
        public IQueryable<QuickOrder> Get([ID] Guid id, [ScopedService] QueryDbContext context)
        {
            SetLogTransaction(id);
            return context.QuickOrders
                .Where(c => c.Id == id && c.UserId == CurrentUser.Id);
        }
        
        [GraphQLName("quickOrders")]
        [GraphQLType(typeof(ListType<QuickOrderType>))]
        [UseDbContext(typeof(QueryDbContext))]
        [Authorize(Policy = Policies.STORE)]
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public IQueryable<QuickOrder> GetAll([ScopedService] QueryDbContext context)
        {
            SetLogTransaction();
            return context.QuickOrders
                .Where(c => c.UserId == CurrentUser.Id);
        }
    }
}