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

namespace Sheaft.GraphQL.Producers
{
    public class ProducersByIdBatchDataLoader : BatchDataLoader<Guid, Producer>
    {
        private readonly IDbContextFactory<QueryDbContext> _contextFactory;
        
        public ProducersByIdBatchDataLoader(
            IDbContextFactory<QueryDbContext> contextFactory,
            IBatchScheduler batchScheduler,
            DataLoaderOptions<Guid> options = null)
            : base(batchScheduler, options)
        {
            _contextFactory = contextFactory;
        }
        
        protected override async Task<IReadOnlyDictionary<Guid, Producer>> LoadBatchAsync(
            IReadOnlyList<Guid> keys,
            CancellationToken token)
        {
            return await _contextFactory.CreateDbContext().Producers.Where(u => keys.Contains(u.Id)).ToDictionaryAsync(c => c.Id, token);
        }
    }
}