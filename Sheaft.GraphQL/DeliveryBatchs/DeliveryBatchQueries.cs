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
using Sheaft.Application.Interfaces.Services;
using Sheaft.Application.Models;
using Sheaft.Application.Security;
using Sheaft.Domain;
using Sheaft.Domain.Enum;
using Sheaft.GraphQL.Types.Outputs;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.GraphQL.DeliveryBatchs
{
    [ExtendObjectType(Name = "Query")]
    public class DeliveryBatchQueries : SheaftQuery
    {
        public DeliveryBatchQueries(
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
            : base(currentUserService, httpContextAccessor)
        {
        }

        [GraphQLName("deliveryBatch")]
        [GraphQLType(typeof(DeliveryBatchType))]
        [UseDbContext(typeof(QueryDbContext))]
        [Authorize(Policy = Policies.PRODUCER)]
        [UseSingleOrDefault]
        public IQueryable<DeliveryBatch> Get([ID] Guid id, [ScopedService] QueryDbContext context)
        {
            SetLogTransaction(id);

            return context.DeliveryBatches
                .Where(c => c.AssignedToId == CurrentUser.Id && c.Id == id);
        }

        [GraphQLName("deliveryBatches")]
        [GraphQLType(typeof(ListType<DeliveryBatchType>))]
        [UseDbContext(typeof(QueryDbContext))]
        [Authorize(Policy = Policies.PRODUCER)]
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public IQueryable<DeliveryBatch> GetAll([ScopedService] QueryDbContext context)
        {
            SetLogTransaction();

            return context.DeliveryBatches
                .Where(c => c.AssignedToId == CurrentUser.Id && c.Status != DeliveryBatchStatus.Cancelled);
        }
    }
}