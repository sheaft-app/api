using System.Linq;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;

namespace Sheaft.Infrastructure.Persistence.Extensions
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
    }
}