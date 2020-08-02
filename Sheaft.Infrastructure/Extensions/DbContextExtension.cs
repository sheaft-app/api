using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Sheaft.Infrastructure.Interop;
using Sheaft.Interop;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Sheaft.Infrastructure
{
    public static class DbContextExtension
    {
        public static bool AllMigrationsApplied(this IAppDbContext context)
        {
            var applied = context.GetService<IHistoryRepository>()
                .GetAppliedMigrations()
                .Select(m => m.MigrationId);

            var total = context.GetService<IMigrationsAssembly>()
                .Migrations
                .Select(m => m.Key);

            return !total.Except(applied).Any();
        }

        public static IQueryable<T> Get<T>(this DbSet<T> dbset, Expression<Func<T, bool>> where = null, bool noTracking = false) where T:class, ITrackRemove
        {
            var query = dbset.Where(d => !d.RemovedOn.HasValue);

            if (noTracking)
                query = query.AsNoTracking();

            if (where == null)
                return query;

            return query.Where(where);
        }
    }
}