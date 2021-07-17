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

namespace Sheaft.GraphQL.Observations
{
    public class ObservationsByIdBatchDataLoader : BatchDataLoader<Guid, Observation>
    {
        private readonly IDbContextFactory<QueryDbContext> _contextFactory;

        public ObservationsByIdBatchDataLoader(
            IDbContextFactory<QueryDbContext> contextFactory,
            IBatchScheduler batchScheduler,
            DataLoaderOptions<Guid> options = null)
            : base(batchScheduler, options)
        {
            _contextFactory = contextFactory;
        }

        protected override async Task<IReadOnlyDictionary<Guid, Observation>> LoadBatchAsync(
            IReadOnlyList<Guid> keys,
            CancellationToken token)
        {
            return await _contextFactory.CreateDbContext().Set<Observation>().Where(u => keys.Contains(u.Id))
                .ToDictionaryAsync(c => c.Id, token);
        }
    }
}