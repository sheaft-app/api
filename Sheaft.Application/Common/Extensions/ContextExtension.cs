using Microsoft.EntityFrameworkCore;
using Sheaft.Domain.Interop;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Sheaft.Application.Interop
{
    public static class ContextExtension
    {
        public static IQueryable<T> Get<T>(this DbSet<T> dbset, Expression<Func<T, bool>> where = null, bool noTracking = false) where T:class, ITrackRemove
        {
            var query = dbset.Where(d => !d.RemovedOn.HasValue);

            if (noTracking)
                query = query.AsNoTracking();

            if (where == null)
                return query;

            return query.Where(where);
        }

        public static IQueryable<T> Get<T>(this IQueryable<T> dbset, Expression<Func<T, bool>> where = null, bool noTracking = false) where T : class, ITrackRemove
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