using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Localization;
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
        private readonly IStringLocalizer<MessageResources> _localizer;
        private readonly ISheaftMediatr _mediatr;

        public AppDbContext(
            DbContextOptions<AppDbContext> options,
            IStringLocalizer<MessageResources> localizer,
            ISheaftMediatr mediatr)
            : base(options)
        {
            _localizer = localizer;
            _mediatr = mediatr;
        }
        
        public DbSet<Agreement> Agreements { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
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
        public DbSet<Product> Products { get; set; }
        public DbSet<QuickOrder> QuickOrders { get; set; }
        public DbSet<Refund> Refunds { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Reward> Rewards { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Transfer> Transfers { get; set; }
        public DbSet<User> Users { get; set; }
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

        public async Task<T> GetByIdAsync<T>(Guid id, CancellationToken token, bool asNoTracking = false)
            where T : class, IIdEntity, ITrackRemove
        {
            var query = Set<T>().Where(c => c.Id == id);
            if (asNoTracking)
                query = query.AsNoTracking();

            var item = await query.SingleOrDefaultAsync(token);
            if (item == null)
            {
                var message = GetExceptionDefaultMessage<T>(MessageKind.NotFound, id);
                throw SheaftException.NotFound(message.Key, message.Value);
            }

            var itemAsRemoved = (ITrackRemove) item;
            if (itemAsRemoved.RemovedOn.HasValue)
            {
                var message = GetExceptionDefaultMessage<T>(MessageKind.Gone, id);
                throw SheaftException.Gone(message.Key, message.Value);
            }

            return item;
        }

        public async Task<T> FindByIdAsync<T>(Guid id, CancellationToken token, bool asNoTracking = false)
            where T : class, IIdEntity, ITrackRemove
        {
            var query = Set<T>().Where(c => c.Id == id && !c.RemovedOn.HasValue);
            if (asNoTracking)
                query = query.AsNoTracking();

            return await query.SingleOrDefaultAsync(token);
        }

        public async Task<IEnumerable<T>> GetByIdsAsync<T>(IEnumerable<Guid> ids, CancellationToken token,
            bool asNoTracking = false) where T : class, IIdEntity, ITrackRemove
        {
            var query = Set<T>().Where(c => ids.Contains(c.Id) && !c.RemovedOn.HasValue);
            if (asNoTracking)
                query = query.AsNoTracking();

            var items = await query.ToListAsync(token);
            if (items?.Any() != true)
            {
                var message = GetExceptionDefaultMessage<T>(MessageKind.NotFound, ids);
                throw SheaftException.NotFound(message.Key, message.Value);
            }

            if (items.Count != ids.Count())
            {
                var message = GetExceptionDefaultMessage<T>(MessageKind.NotFound, ids.Except(items.Select(i => i.Id)));
                throw SheaftException.NotFound(message.Key, message.Value);
            }

            return items;
        }

        public async Task<IEnumerable<T>> FindByIdsAsync<T>(IEnumerable<Guid> ids, CancellationToken token,
            bool asNoTracking = false) where T : class, IIdEntity, ITrackRemove
        {
            var query = Set<T>().Where(c => ids.Contains(c.Id) && !c.RemovedOn.HasValue);
            if (asNoTracking)
                query = query.AsNoTracking();

            var items = await query.ToListAsync(token);
            if (items?.Any() != true)
                return new List<T>();

            return items;
        }

        public async Task<IEnumerable<T>> GetAsync<T>(Expression<Func<T, bool>> where, CancellationToken token,
            bool asNoTracking = false) where T : class, ITrackRemove
        {
            var query = Set<T>().Where(c => !c.RemovedOn.HasValue);
            if (where != null)
                query = query.Where(where);

            if (asNoTracking)
                query = query.AsNoTracking();

            var items = await query.ToListAsync(token);
            if (items?.Any() != true)
            {
                var message = GetExceptionDefaultMessage<T>(MessageKind.NotFound);
                throw SheaftException.NotFound(message.Key, message.Value);
            }

            return items;
        }

        public async Task<IEnumerable<T>> GetAsync<T>(CancellationToken token, bool asNoTracking = false)
            where T : class, ITrackRemove
        {
            return await GetAsync<T>(null, token, asNoTracking);
        }

        public async Task<IEnumerable<T>> FindAsync<T>(Expression<Func<T, bool>> where, CancellationToken token,
            bool asNoTracking = false) where T : class, ITrackRemove
        {
            var query = Set<T>().Where(c => !c.RemovedOn.HasValue);
            if (where != null)
                query = query.Where(where);

            if (asNoTracking)
                query = query.AsNoTracking();

            var items = await query.ToListAsync(token);
            if (items?.Any() != true)
                return new List<T>();

            return items;
        }

        public async Task<IEnumerable<T>> FindAsync<T>(CancellationToken token, bool asNoTracking = false)
            where T : class, ITrackRemove
        {
            return await FindAsync<T>(null, token, asNoTracking);
        }

        public async Task<T> GetSingleAsync<T>(Expression<Func<T, bool>> where, CancellationToken token,
            bool asNoTracking = false) where T : class, ITrackRemove
        {
            var query = Set<T>().Where(c => !c.RemovedOn.HasValue);
            if (where != null)
                query = query.Where(where);

            if (asNoTracking)
                query = query.AsNoTracking();

            var item = await query.SingleOrDefaultAsync(token);
            if (item == null)
            {
                var message = GetExceptionDefaultMessage<T>(MessageKind.NotFound);
                throw SheaftException.NotFound(message.Key, message.Value);
            }

            return item;
        }

        public async Task<T> FindSingleAsync<T>(Expression<Func<T, bool>> where, CancellationToken token,
            bool asNoTracking = false) where T : class, ITrackRemove
        {
            var query = Set<T>().Where(c => !c.RemovedOn.HasValue);
            if (where != null)
                query = query.Where(where);

            if (asNoTracking)
                query = query.AsNoTracking();

            return await query.SingleOrDefaultAsync(token);
        }

        public async Task<bool> AnyAsync<T>(Expression<Func<T, bool>> where, CancellationToken token,
            bool asNoTracking = false) where T : class, ITrackRemove
        {
            var query = Set<T>().Where(c => !c.RemovedOn.HasValue);
            if (where != null)
                query = query.Where(where);

            if (asNoTracking)
                query = query.AsNoTracking();

            return await query.AnyAsync(token);
        }

        public async Task<bool> AnyAsync<T>(CancellationToken token, bool asNoTracking = false)
            where T : class, ITrackRemove
        {
            return await AnyAsync<T>(null, token, asNoTracking);
        }

        public async Task EnsureNotExists<T>(Guid id, CancellationToken token, bool asNoTracking = false)
            where T : class, IIdEntity, ITrackRemove
        {
            var query = Set<T>().Where(c => !c.RemovedOn.HasValue && c.Id == id);
            if (asNoTracking)
                query = query.AsNoTracking();

            var result = await query.SingleOrDefaultAsync(token);
            if (result != null)
            {
                var message = GetExceptionDefaultMessage<T>(MessageKind.AlreadyExists, id);
                throw SheaftException.AlreadyExists(message.Key, message.Value);
            }
        }

        public async Task EnsureNotExists<T>(Expression<Func<T, bool>> where, CancellationToken token,
            bool asNoTracking = false) where T : class, IIdEntity, ITrackRemove
        {
            var query = Set<T>().Where(c => !c.RemovedOn.HasValue);
            if (where != null)
                query = query.Where(where);

            if (asNoTracking)
                query = query.AsNoTracking();

            var result = await query.ToListAsync(token);
            if (result?.Any() == true)
            {
                var message = GetExceptionDefaultMessage<T>(MessageKind.AlreadyExists);
                throw SheaftException.AlreadyExists(message.Key, message.Value);
            }
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

        private KeyValuePair<MessageKind, string> GetExceptionDefaultMessage<T>(MessageKind kind, Guid id)
            where T : class, IIdEntity, ITrackRemove
        {
            var type = typeof(T);
            var resource = $"{_localizer[type.Name, type.Name]} ({id})";
            return new KeyValuePair<MessageKind, string>(kind, resource);
        }

        private KeyValuePair<MessageKind, string> GetExceptionDefaultMessage<T>(MessageKind kind)
            where T : class, ITrackRemove
        {
            var type = typeof(T);
            var resource = $"{_localizer[type.Name, type.Name]}";
            return new KeyValuePair<MessageKind, string>(kind, resource);
        }

        private KeyValuePair<MessageKind, string> GetExceptionDefaultMessage<T>(MessageKind kind, IEnumerable<Guid> ids)
            where T : class, ITrackRemove
        {
            var type = typeof(T);
            var identifiers = string.Empty;

            foreach (var id in ids)
            {
                if (identifiers.Length > 0)
                    identifiers += ", ";

                identifiers += id.ToString("N");
            }

            var resource = $"{_localizer[type.Name, type.Name]} ({identifiers})";
            return new KeyValuePair<MessageKind, string>(kind, resource);
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