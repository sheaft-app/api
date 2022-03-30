using Microsoft.EntityFrameworkCore;
using Sheaft.Domain;
using Sheaft.Domain.AccountManagement;
using Sheaft.Domain.SupplierManagement;
using Sheaft.Infrastructure.Persistence.Configurations;
using Sheaft.Infrastructure.Persistence.Converters;

namespace Sheaft.Infrastructure.Persistence;

public interface IDbContext
{
    public DbSet<TEntity> Set<TEntity>() where TEntity : class;
}

internal class AppDbContext : DbContext, IDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new AccountConfiguration());
        modelBuilder.ApplyConfiguration(new SupplierConfiguration());
        modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
    }
    
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<SupplierId>()
            .HaveConversion<SupplierIdConverter>();
        
        configurationBuilder
            .Properties<AccountId>()
            .HaveConversion<AccountIdConverter>();
        
        configurationBuilder
            .Properties<RefreshTokenId>()
            .HaveConversion<RefreshTokenIdConverter>();
        
        configurationBuilder
            .Properties<EmailAddress>()
            .HaveConversion<EmailAddressConverter>();
        
        configurationBuilder
            .Properties<PhoneNumber>()
            .HaveConversion<PhoneNumberConverter>();
        
        configurationBuilder
            .Properties<Username>()
            .HaveConversion<UsernameConverter>();
        
        configurationBuilder
            .Properties<TradeName>()
            .HaveConversion<TradeNameConverter>();
        
        configurationBuilder
            .Properties<CorporateName>()
            .HaveConversion<CorporateNameConverter>();
    }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
}