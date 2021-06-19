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
using Sheaft.GraphQL.Types.Outputs;
using Sheaft.Infrastructure.Persistence;
using Sheaft.Options;

namespace Sheaft.GraphQL.PurchaseOrderDeliveries
{
    [ExtendObjectType(Name = "Query")]
    public class PurchaseOrderDeliveryQueries : SheaftQuery
    {
        private readonly RoleOptions _roleOptions;

        public PurchaseOrderDeliveryQueries(
            IOptionsSnapshot<RoleOptions> roleOptions,
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            :base(currentUserService, httpContextAccessor)
        {
            _roleOptions = roleOptions.Value;
        }

        [GraphQLName("purchaseOrderDelivery")]
        [GraphQLType(typeof(PurchaseOrderDeliveryType))]
        [Authorize(Policy = Policies.REGISTERED)]
        [UseDbContext(typeof(QueryDbContext))]
        [UseSingleOrDefault]
        public IQueryable<PurchaseOrderDelivery> GetPurchaseOrderDelivery([ID] Guid id,
            [ScopedService] QueryDbContext context)
        {
            SetLogTransaction(CurrentUser.Id);
            
            return context.Set<PurchaseOrderDelivery>()
                .Where(c => c.Id == id);
        }

        [GraphQLName("purchaseOrderDeliveries")]
        [GraphQLType(typeof(ListType<PurchaseOrderDeliveryType>))]
        [UseDbContext(typeof(QueryDbContext))]
        [Authorize(Policy = Policies.REGISTERED)]
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public IQueryable<PurchaseOrderDelivery> GetAll([ScopedService] QueryDbContext context, CancellationToken token)
        {
            SetLogTransaction();

            if (CurrentUser.IsInRole(_roleOptions.Producer.Value))
                return context.Set<PurchaseOrderDelivery>()
                    .Where(c => c.PurchaseOrder.ProducerId == CurrentUser.Id);
            
            return context.Set<PurchaseOrderDelivery>()
                .Where(c => c.PurchaseOrder.ClientId == CurrentUser.Id);
        }
    }
}