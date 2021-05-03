using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut;
using HotChocolate.DataLoader;
using Microsoft.EntityFrameworkCore;
using Sheaft.Domain;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.GraphQL.PurchaseOrders
{
    public class PurchaseOrdersByIdBatchDataLoader : BatchDataLoader<Guid, PurchaseOrder>
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;

        public PurchaseOrdersByIdBatchDataLoader(
            IDbContextFactory<AppDbContext> contextFactory,
            IBatchScheduler batchScheduler,
            DataLoaderOptions<Guid> options = null)
            : base(batchScheduler, options)
        {
            _contextFactory = contextFactory;
        }

        protected override async Task<IReadOnlyDictionary<Guid, PurchaseOrder>> LoadBatchAsync(
            IReadOnlyList<Guid> keys,
            CancellationToken token)
        {
            return await _contextFactory.CreateDbContext().PurchaseOrders
                .Where(u => keys.Contains(u.Id))
                .ToDictionaryAsync(c => c.Id, token);
        }
    }
}