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

namespace Sheaft.GraphQL.Deliveries
{
    public class DeliveryReturnablesByIdBatchDataLoader : BatchDataLoader<Guid, DeliveryReturnable>
    {
        private readonly IDbContextFactory<QueryDbContext> _contextFactory;

        public DeliveryReturnablesByIdBatchDataLoader(
            IDbContextFactory<QueryDbContext> contextFactory,
            IBatchScheduler batchScheduler,
            DataLoaderOptions<Guid> options = null)
            : base(batchScheduler, options)
        {
            _contextFactory = contextFactory;
        }

        protected override async Task<IReadOnlyDictionary<Guid, DeliveryReturnable>> LoadBatchAsync(
            IReadOnlyList<Guid> keys,
            CancellationToken token)
        {
            return await _contextFactory.CreateDbContext().Set<DeliveryReturnable>()
                .Where(u => keys.Contains(u.Id))
                .ToDictionaryAsync(c => c.Id, token);
        }
    }
}