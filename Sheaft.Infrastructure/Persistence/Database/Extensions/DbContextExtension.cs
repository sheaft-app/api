using System.Linq;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence.Database.Extensions
{
    public static class DbContextExtension
    {
        public static bool AllMigrationsApplied(this AppDbContext context)
        {
            var applied = AccessorExtensions.GetService<IHistoryRepository>(context)
                .GetAppliedMigrations()
                .Select(m => m.MigrationId);

            var total = AccessorExtensions.GetService<IMigrationsAssembly>(context)
                .Migrations
                .Select(m => m.Key);
            
            return !total.Except(applied).Any();
        }
    }
}