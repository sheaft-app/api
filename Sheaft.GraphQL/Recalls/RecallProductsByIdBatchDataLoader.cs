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

namespace Sheaft.GraphQL.Recalls
{
    public class RecallProductsByIdBatchDataLoader : BatchDataLoader<Guid, RecallProduct>
    {
        private readonly IDbContextFactory<QueryDbContext> _contextFactory;

        public RecallProductsByIdBatchDataLoader(
            IDbContextFactory<QueryDbContext> contextFactory,
            IBatchScheduler batchScheduler,
            DataLoaderOptions<Guid> options = null)
            : base(batchScheduler, options)
        {
            _contextFactory = contextFactory;
        }

        protected override async Task<IReadOnlyDictionary<Guid, RecallProduct>> LoadBatchAsync(
            IReadOnlyList<Guid> keys,
            CancellationToken token)
        {
            return await _contextFactory.CreateDbContext().Set<RecallProduct>().Where(u => keys.Contains(u.Id))
                .ToDictionaryAsync(c => c.Id, token);
        }
    }
}