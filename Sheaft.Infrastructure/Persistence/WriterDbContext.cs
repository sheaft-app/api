using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Sheaft.Application.Interfaces.Mediatr;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Domain.Common;
using Sheaft.Domain.Interop;

namespace Sheaft.Infrastructure.Persistence
{
    public partial class WriterDbContext : QueryDbContext, IAppDbContext
    {
        private readonly ISheaftMediatr _mediatr;
        private readonly bool _isAdminContext;

        public WriterDbContext(
            ISheaftMediatr mediatr,
            IConfiguration configuration,
            DbContextOptions options)
            : base(options)
        {
            _mediatr = mediatr;
            _isAdminContext = configuration.GetValue<bool?>("IsAdminContext") ?? false;
        }
        
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
                    var removedOnProperty = entry.Property("RemovedOn");
                    if (removedOnProperty.CurrentValue == null)
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