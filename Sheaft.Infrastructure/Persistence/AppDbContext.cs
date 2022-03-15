using Microsoft.EntityFrameworkCore;
using Sheaft.Domain.AccountManagement;
using Sheaft.Infrastructure.Persistence.Configurations;

namespace Sheaft.Infrastructure.Persistence;

internal interface IDbContext
{
    public DbSet<TEntity> Set<TEntity>() where TEntity : class;
}

public class AppDbContext : DbContext, IDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new AccountConfiguration());
        modelBuilder.ApplyConfiguration(new ProfileConfiguration());
        modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
    }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<Profile> Profiles { get; set; }
}