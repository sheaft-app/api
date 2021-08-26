using System;
using System.Linq;
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

namespace Sheaft.GraphQL.PurchaseOrders
{
    [ExtendObjectType(Name = "Query")]
    public class PurchaseOrderQueries : SheaftQuery
    {
        private readonly RoleOptions _roleOptions;

        public PurchaseOrderQueries(
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor,
            IOptionsSnapshot<RoleOptions> roleOptions)
            :base(currentUserService, httpContextAccessor)
        {
            _roleOptions = roleOptions.Value;
        }

        [GraphQLName("purchaseOrder")]
        [GraphQLType(typeof(PurchaseOrderType))]
        [UseDbContext(typeof(QueryDbContext))]
        [Authorize(Policy = Policies.REGISTERED)]
        [UseSingleOrDefault]
        public IQueryable<PurchaseOrder> Get([ID] Guid id, [ScopedService] QueryDbContext context)
        {
            SetLogTransaction(id);
            if (CurrentUser.IsInRole(_roleOptions.Producer.Value))
            {
                return context.PurchaseOrders
                    .Where(c => c.Id == id && c.ProducerId == CurrentUser.Id);
            }

            return context.PurchaseOrders
                .Where(c => c.Id == id && c.ClientId == CurrentUser.Id);
        }
        
        [GraphQLName("purchaseOrders")]
        [GraphQLType(typeof(ListType<PurchaseOrderType>))]
        [UseDbContext(typeof(QueryDbContext))]
        [Authorize(Policy = Policies.REGISTERED)]
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public IQueryable<PurchaseOrder> GetAll([ScopedService] QueryDbContext context)
        {
            SetLogTransaction();
            if (CurrentUser.IsInRole(_roleOptions.Producer.Value))
            {
                return context.PurchaseOrders
                    .Where(c => c.ProducerId == CurrentUser.Id);
            }

            return context.PurchaseOrders
                .Where(c => c.ClientId == CurrentUser.Id);
        }
    }
}