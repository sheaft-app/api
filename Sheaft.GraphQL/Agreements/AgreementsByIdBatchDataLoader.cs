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

namespace Sheaft.GraphQL.DataLoaders
{
    public class AgreementsByIdBatchDataLoader : BatchDataLoader<Guid, Agreement>
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;
        
        public AgreementsByIdBatchDataLoader(
            IDbContextFactory<AppDbContext> contextFactory,
            IBatchScheduler batchScheduler,
            DataLoaderOptions<Guid> options = null)
            : base(batchScheduler, options)
        {
            _contextFactory = contextFactory;
        }
        
        protected override async Task<IReadOnlyDictionary<Guid, Agreement>> LoadBatchAsync(
            IReadOnlyList<Guid> keys,
            CancellationToken token)
        {
            return await _contextFactory.CreateDbContext().Agreements.Where(u => keys.Contains(u.Id)).ToDictionaryAsync(c => c.Id, token);
        }
    }
}