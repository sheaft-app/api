using System;
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
using Sheaft.Application.Interfaces.Services;
using Sheaft.Application.Security;
using Sheaft.Core.Options;
using Sheaft.Domain;
using Sheaft.GraphQL.Types.Outputs;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.GraphQL.Batches
{
    [ExtendObjectType(Name = "Query")]
    public class BatchQueries : SheaftQuery
    {
        private readonly RoleOptions _roleOptions;

        public BatchQueries(
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor,
            IOptionsSnapshot<RoleOptions> roleOptions)
            : base(currentUserService, httpContextAccessor)
        {
            _roleOptions = roleOptions.Value;
        }

        [GraphQLName("batch")]
        [GraphQLType(typeof(BatchType))]
        [UseDbContext(typeof(QueryDbContext))]
        [Authorize(Policy = Policies.REGISTERED)]
        [UseSingleOrDefault]
        public IQueryable<Batch> Get([ID] Guid id, [ScopedService] QueryDbContext context)
        {
            SetLogTransaction(id);
            return context.Batches
                .Where(c => c.Id == id);
        }

        [GraphQLName("batches")]
        [GraphQLType(typeof(ListType<BatchType>))]
        [UseDbContext(typeof(QueryDbContext))]
        [Authorize(Policy = Policies.REGISTERED)]
        [UsePaging]
        [UseFiltering]
        [UseSorting]
        public async Task<IQueryable<Batch>> GetAll([ScopedService] QueryDbContext context, CancellationToken token,
            bool validOnly = false)
        {
            SetLogTransaction();

            if (CurrentUser.IsInRole(_roleOptions.Producer.Value))
            {
                if (validOnly)
                    return context.Batches
                        .Where(c => c.ProducerId == CurrentUser.Id && ((c.DLC.HasValue && c.DLC > DateTime.UtcNow) ||
                                                                       (c.DDM.HasValue && c.DDM > DateTime.UtcNow)));

                return context.Batches
                    .Where(c => c.ProducerId == CurrentUser.Id);
            }

            var batchIds = await context.PurchaseOrders
                .Where(c => c.ClientId == CurrentUser.Id)
                .SelectMany(c =>
                    c.Picking.PreparedProducts.Where(pp => pp.PurchaseOrderId == c.Id)
                        .SelectMany(pp => pp.Batches.Select(b => b.BatchId))
                ).ToListAsync(token);

            if (validOnly)
                return context.Batches
                    .Where(c => batchIds.Contains(c.Id) && ((c.DLC.HasValue && c.DLC > DateTime.UtcNow) ||
                                                                   (c.DDM.HasValue && c.DDM > DateTime.UtcNow)));

            return context.Batches
                .Where(c => batchIds.Contains(c.Id));
        }
    }
}