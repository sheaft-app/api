using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Sheaft.Application.Mediator;
using Sheaft.Domain;
using Sheaft.Domain.Events;
using Sheaft.Domain.Interop;
using Sheaft.Infrastructure.Persistence.Database;

namespace Sheaft.Infrastructure.Persistence
{
    public partial class AppDbContext : DbContext
    {
        private readonly IMediator _mediatr;
        private readonly bool _isAdminContext;

        public AppDbContext(
            IMediator mediatr,
            IConfiguration configuration,
            DbContextOptions options)
            : base(options)
        {
            _mediatr = mediatr;
            _isAdminContext = configuration.GetValue<bool?>("IsAdminContext") ?? false;
        }

        public DbSet<BatchNumber> BatchNumbers { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Catalog> Catalogs { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<Distribution> Distributions { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Observation> Observations { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PickingOrder> PickingOrders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<QuickOrder> QuickOrders { get; set; }
        public DbSet<Recall> Recalls { get; set; }
        public DbSet<Returnable> Returnables { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<User> Users { get; set; }

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

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken token = default)
        {
            if (Database.CurrentTransaction != null)
                return new InnerTransaction();

            return new InnerTransaction(await Database.BeginTransactionAsync(token), DispatchEvents);
        }

        internal void Migrate()
        {
            Database.Migrate();
        }

        private void UpdateRelatedData()
        {
            ChangeTracker.DetectChanges();

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Unchanged)
                    continue;
                
                if (entry.Entity is ITrackCreation)
                {
                    var createdOnProperty = entry.Property("CreatedOn");
                    if ((DateTimeOffset) createdOnProperty.CurrentValue == default && entry.State != EntityState.Deleted)
                    {
                        entry.State = EntityState.Added;
                        createdOnProperty.CurrentValue = DateTimeOffset.UtcNow;
                    }
                }

                if (entry.Entity is ITrackUpdate)
                {
                    var updatedOnProperty = entry.Property("UpdatedOn");
                    updatedOnProperty.CurrentValue = DateTimeOffset.UtcNow;
                }

                if (entry.State == EntityState.Deleted && entry.Entity is ITrackRemove)
                {
                    var removedProperty = entry.Property("Removed");
                    if (removedProperty.CurrentValue == null || !(bool)removedProperty.CurrentValue)
                        removedProperty.CurrentValue = true;

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