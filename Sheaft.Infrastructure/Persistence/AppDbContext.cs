using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Sheaft.Domain;
using Sheaft.Domain.AccountManagement;
using Sheaft.Domain.AgreementManagement;
using Sheaft.Domain.ProductManagement;
using Sheaft.Domain.OrderManagement;
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
        modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
        
        modelBuilder.ApplyConfiguration(new SupplierConfiguration());
        modelBuilder.ApplyConfiguration(new CustomerConfiguration());
        
        modelBuilder.ApplyConfiguration(new CatalogConfiguration());
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new CatalogProductConfiguration());
        
        modelBuilder.ApplyConfiguration(new AgreementConfiguration());
        
        modelBuilder.ApplyConfiguration(new OrderConfiguration());
        modelBuilder.ApplyConfiguration(new DeliveryConfiguration());
    }
    
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<SupplierId>()
            .HaveConversion<SupplierIdConverter>();
        
        configurationBuilder
            .Properties<CustomerId>()
            .HaveConversion<CustomerIdConverter>();
        
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
            .Properties<CatalogId>()
            .HaveConversion<CatalogIdConverter>();
        
        configurationBuilder
            .Properties<ProductId>()
            .HaveConversion<ProductIdConverter>();
        
        configurationBuilder
            .Properties<AgreementId>()
            .HaveConversion<AgreementIdConverter>();
        
        configurationBuilder
            .Properties<OrderId>()
            .HaveConversion<OrderIdConverter>();
        
        configurationBuilder
            .Properties<DeliveryId>()
            .HaveConversion<DeliveryIdConverter>();
    }

    public DbSet<Account> Accounts { get; set; }
    
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Customer> Customers { get; set; }
    
    public DbSet<Catalog> Catalogs { get; set; }
    public DbSet<Product> Products { get; set; }
    
    public DbSet<Agreement> Agreements { get; set; }
    
    public DbSet<Order> Orders { get; set; }
    public DbSet<Delivery> Deliveries { get; set; }
}

internal static class DbContextExtension
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