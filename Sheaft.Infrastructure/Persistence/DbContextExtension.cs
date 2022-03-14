using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sheaft.Infrastructure.Persistence;

public static class DbContextExtension
{
    public static bool AllMigrationsApplied(this AppDbContext context)
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