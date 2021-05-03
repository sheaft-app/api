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

namespace Sheaft.GraphQL.Stores
{
    public class StoresByIdBatchDataLoader : BatchDataLoader<Guid, Store>
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;
        
        public StoresByIdBatchDataLoader(
            IDbContextFactory<AppDbContext> contextFactory,
            IBatchScheduler batchScheduler,
            DataLoaderOptions<Guid> options = null)
            : base(batchScheduler, options)
        {
            _contextFactory = contextFactory;
        }
        
        protected override async Task<IReadOnlyDictionary<Guid, Store>> LoadBatchAsync(
            IReadOnlyList<Guid> keys,
            CancellationToken token)
        {
            return await _contextFactory.CreateDbContext().Stores.Where(u => keys.Contains(u.Id)).ToDictionaryAsync(c => c.Id, token);
        }
    }
}