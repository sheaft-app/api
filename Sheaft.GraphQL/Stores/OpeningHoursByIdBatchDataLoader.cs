using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut;
using HotChocolate.DataLoader;
using Microsoft.EntityFrameworkCore;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.GraphQL.Stores
{
    public class OpeningHoursByIdBatchDataLoader : BatchDataLoader<Guid, Domain.OpeningHours>
    {
        private readonly IDbContextFactory<QueryDbContext> _contextFactory;

        public OpeningHoursByIdBatchDataLoader(
            IDbContextFactory<QueryDbContext> contextFactory,
            IBatchScheduler batchScheduler,
            DataLoaderOptions<Guid> options = null)
            : base(batchScheduler, options)
        {
            _contextFactory = contextFactory;
        }

        protected override async Task<IReadOnlyDictionary<Guid, Domain.OpeningHours>> LoadBatchAsync(
            IReadOnlyList<Guid> keys,
            CancellationToken token)
        {
            return await _contextFactory.CreateDbContext().Set<Domain.OpeningHours>().Where(u => keys.Contains(u.Id))
                .ToDictionaryAsync(c => c.Id, token);
        }
    }
}