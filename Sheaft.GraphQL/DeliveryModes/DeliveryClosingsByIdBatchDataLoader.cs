using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut;
using HotChocolate.DataLoader;
using Microsoft.EntityFrameworkCore;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.GraphQL.DeliveryModes
{
    public class DeliveryClosingsByIdBatchDataLoader : BatchDataLoader<Guid, Domain.DeliveryClosing>
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;

        public DeliveryClosingsByIdBatchDataLoader(
            IDbContextFactory<AppDbContext> contextFactory,
            IBatchScheduler batchScheduler,
            DataLoaderOptions<Guid> options = null)
            : base(batchScheduler, options)
        {
            _contextFactory = contextFactory;
        }

        protected override async Task<IReadOnlyDictionary<Guid, Domain.DeliveryClosing>> LoadBatchAsync(
            IReadOnlyList<Guid> keys,
            CancellationToken token)
        {
            return await _contextFactory.CreateDbContext().Set<Domain.DeliveryClosing>().Where(u => keys.Contains(u.Id))
                .ToDictionaryAsync(c => c.Id, token);
        }
    }
}