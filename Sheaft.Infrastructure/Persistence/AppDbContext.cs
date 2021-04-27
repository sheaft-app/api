using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain;
using Sheaft.Domain.Common;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Interop;
using Sheaft.Domain.Views;
using Sheaft.Localization;

namespace Sheaft.Infrastructure.Persistence
{
    public partial class AppDbContext : DbContext, IAppDbContext
    {
        private ISheaftMediatr _mediatr => this.GetService<ISheaftMediatr>();
        private bool _isAdminContext => this.GetService<IConfiguration>().GetValue<bool?>("IsAdminContext") ?? false;

        public AppDbContext(
            DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<Agreement> Agreements { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Catalog> Catalogs { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<DeliveryMode> DeliveryModes { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Legal> Legals { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<Nationality> Nationalities { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<Returnable> Returnables { get; set; }
        public DbSet<Payin> Payins { get; set; }
        public DbSet<Payout> Payouts { get; set; }
        public DbSet<PreAuthorization> PreAuthorizations { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<QuickOrder> QuickOrders { get; set; }
        public DbSet<Refund> Refunds { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Reward> Rewards { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Transfer> Transfers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Producer> Producers { get; set; }
        public DbSet<Consumer> Consumers { get; set; }
        public DbSet<Business> Businesses { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Withholding> Withholdings { get; set; }

        public DbSet<DepartmentProducers> DepartmentProducers { get; set; }
        public DbSet<DepartmentStores> DepartmentStores { get; set; }
        public DbSet<CountryPoints> CountryPoints { get; set; }
        public DbSet<RegionPoints> RegionPoints { get; set; }
        public DbSet<DepartmentPoints> DepartmentPoints { get; set; }
        public DbSet<DepartmentUserPoints> DepartmentUserPoints { get; set; }
        public DbSet<RegionUserPoints> RegionUserPoints { get; set; }
        public DbSet<CountryUserPoints> CountryUserPoints { get; set; }

        public override int SaveChanges()
        {
            UpdateRelatedData();
            return base.SaveChanges(true);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            UpdateRelatedData();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateRelatedData();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            UpdateRelatedData();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public EntityEntry<TEntity> Restore<TEntity>(TEntity entity) where TEntity : class, ITrackRemove
        {
            var entry = Attach(entity);

            var removedOnProperty = entry.Property("RemovedOn");
            if (removedOnProperty != null)
                removedOnProperty.CurrentValue = null;

            entry.State = EntityState.Modified;
            return entry;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken token = default)
        {
            if (Database.CurrentTransaction != null)
                return new InnerTransaction();

            return new InnerTransaction(await Database.BeginTransactionAsync(token), DispatchEvents);
        }

        public void Migrate()
        {
            Database.Migrate();
        }

        private void UpdateRelatedData()
        {
            ChangeTracker.DetectChanges();

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added && entry.Entity is ITrackCreation)
                {
                    var createdOnProperty = entry.Property("CreatedOn");
                    if (createdOnProperty != null)
                        createdOnProperty.CurrentValue = DateTimeOffset.UtcNow;
                }

                if (entry.State == EntityState.Modified && entry.Entity is ITrackUpdate)
                {
                    var updatedOnProperty = entry.Property("UpdatedOn");
                    if (updatedOnProperty != null)
                        updatedOnProperty.CurrentValue = DateTimeOffset.UtcNow;
                }

                if (entry.State == EntityState.Deleted && entry.Entity is ITrackRemove)
                {
                    var removedOnProperty = entry.Property("RemovedOn");
                    if (removedOnProperty != null && removedOnProperty.CurrentValue == null)
                        removedOnProperty.CurrentValue = DateTimeOffset.UtcNow;

                    entry.State = EntityState.Modified;
                }
            }
            
            if(Database.CurrentTransaction == null)
                DispatchEvents();
        }

        private void DispatchEvents()
        {
            while (true)
            {
                var domainEventEntity = ChangeTracker.Entries<IHasDomainEvent>()
                    .Select(x => x.Entity.DomainEvents)
                    .SelectMany(x => x)
                    .FirstOrDefault(domainEvent => !domainEvent.IsPublished);

                if (domainEventEntity == null)
                    break;

                domainEventEntity.IsPublished = true;
                _mediatr.Post(domainEventEntity);
            }
        }
    }
}