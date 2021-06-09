using System;
using System.Collections.Generic;
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
using Sheaft.Domain.Enum;
using Sheaft.GraphQL.Types.Outputs;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.GraphQL.Orders
{
    [ExtendObjectType(Name = "Query")]
    public class OrderQueries : SheaftQuery
    {
        public OrderQueries(
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            : base(currentUserService, httpContextAccessor)
        {
        }

        [GraphQLName("order")]
        [GraphQLType(typeof(OrderType))]
        [UseDbContext(typeof(QueryDbContext))]
        [UseSingleOrDefault]
        public IQueryable<Order> Get([ID] Guid id, [ScopedService] QueryDbContext context)
        {
            SetLogTransaction(id);
            if (CurrentUser.IsAuthenticated())
                return context.Orders
                    .Where(c => c.Id == id && c.UserId == CurrentUser.Id);

            return context.Orders
                .Where(c => c.Id == id && c.Status == OrderStatus.Created && !c.UserId.HasValue);
        }

        [GraphQLName("orderFromTransaction")]
        [GraphQLType(typeof(OrderType))]
        [UseDbContext(typeof(QueryDbContext))]
        [UseSingleOrDefault]
        public IQueryable<Order> Get(string identifier, [ScopedService] QueryDbContext context)
        {
            SetLogTransaction(identifier);
            
            return context.PreAuthorizations
                .Where(p => p.Identifier == identifier)
                .Select(p => p.Order);
        }

        [GraphQLName("currentOrder")]
        [GraphQLType(typeof(OrderType))]
        [UseDbContext(typeof(QueryDbContext))]
        [Authorize(Policy = Policies.REGISTERED)]
        [UseFirstOrDefault]
        public IQueryable<Order> Get([ScopedService] QueryDbContext context)
        {
            SetLogTransaction();
            if (CurrentUser.IsAuthenticated())
                return context.Orders
                    .Where(c => c.UserId == CurrentUser.Id && c.Status == OrderStatus.Created)
                    .OrderByDescending(c => c.CreatedOn);

            return new List<Order>().AsQueryable();
        }

        [GraphQLName("orders")]
        [GraphQLType(typeof(ListType<OrderType>))]
        [UseDbContext(typeof(QueryDbContext))]
        [Authorize(Policy = Policies.REGISTERED)]
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Order> GetAll([ScopedService] QueryDbContext context)
        {
            SetLogTransaction();

            if (CurrentUser.IsAuthenticated())
                return context.Orders
                    .Where(o => o.UserId == CurrentUser.Id);

            return new List<Order>().AsQueryable();
        }
    }
}