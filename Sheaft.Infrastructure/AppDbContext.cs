using System;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Interop;
using Sheaft.Infrastructure.Interop;
using Sheaft.Domain.Models;
using Sheaft.Domain.Views;
using Microsoft.EntityFrameworkCore;
using Sheaft.Exceptions;
using Sheaft.Interop.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Extensions.Localization;
using Sheaft.Localization;
using Microsoft.Extensions.Logging;

namespace Sheaft.Infrastructure
{
    public partial class AppDbContext : DbContext, IAppDbContext
    {
        private readonly IStringLocalizer<MessageResources> _localizer;
        private readonly ILogger<AppDbContext> _logger;
        public AppDbContext(
            DbContextOptions<AppDbContext> options, 
            IStringLocalizer<MessageResources> localizer, 
            ILogger<AppDbContext> logger) 
                : base(options)
        {
            _logger = logger;
            _localizer = localizer;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Agreement> Agreements { get; set; }
        public DbSet<DeliveryMode> DeliveryModes { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<Reward> Rewards { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<Packaging> Packagings { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<QuickOrder> QuickOrders { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public DbSet<DepartmentProducers> DepartmentProducers { get; set; }
        public DbSet<DepartmentStores> DepartmentStores { get; set; }
        public DbSet<CountryPoints> CountryPoints { get; set; }
        public DbSet<RegionPoints> RegionPoints { get; set; }
        public DbSet<DepartmentPoints> DepartmentPoints { get; set; }
        public DbSet<DepartmentUserPoints> DepartmentUserPoints { get; set; }
        public DbSet<RegionUserPoints> RegionUserPoints { get; set; }
        public DbSet<CountryUserPoints> CountryUserPoints { get; set; }

        public async Task<T> GetByIdAsync<T>(Guid id, CancellationToken token) where T : class, IIdEntity, ITrackRemove
        {
            var item = await Set<T>().SingleOrDefaultAsync(c => c.Id == id, token);
            if (item == null)
            {
                var message = GetExceptionDefaultMessage<T>(MessageKind.NotFound, id);
                throw new SheaftException(ExceptionKind.NotFound, message.Key, message.Value);
            }

            var itemAsRemoved = (ITrackRemove)item;
            if (itemAsRemoved.RemovedOn.HasValue)
            {
                var message = GetExceptionDefaultMessage<T>(MessageKind.Gone, id);
                throw new SheaftException(ExceptionKind.Gone, message.Key, message.Value);
            }

            return item;
        }

        public async Task<T> FindByIdAsync<T>(Guid id, CancellationToken token) where T : class, IIdEntity, ITrackRemove
        {
            return await Set<T>().SingleOrDefaultAsync(c => c.Id == id && !c.RemovedOn.HasValue, token);
        }

        public async Task<IEnumerable<T>> GetByIdsAsync<T>(IEnumerable<Guid> ids, CancellationToken token) where T : class, IIdEntity, ITrackRemove
        {
            var items = await Set<T>().Where(c => ids.Contains(c.Id) && !c.RemovedOn.HasValue).ToListAsync(token);
            if (items == null || !items.Any())
                throw new SheaftException(ExceptionKind.NotFound, new Exception(ids.ToString()));

            if(items.Count() != ids.Count())
                throw new SheaftException(ExceptionKind.NotFound, new Exception(ids.Except(items.Select(i => i.Id)).ToString()));

            return items;
        }

        public async Task<IEnumerable<T>> FindByIdsAsync<T>(IEnumerable<Guid> ids, CancellationToken token) where T : class, IIdEntity, ITrackRemove
        {
            var items = await Set<T>().Where(c => ids.Contains(c.Id) && !c.RemovedOn.HasValue).ToListAsync(token);
            if (items == null || !items.Any())
                return new List<T>();

            return items;
        }

        public async Task<IEnumerable<T>> GetAsync<T>(Expression<Func<T, bool>> where, CancellationToken token) where T : class, ITrackRemove
        {
            var items = await Set<T>().Where(c => !c.RemovedOn.HasValue).Where(where).ToListAsync(token);
            if (items == null || !items.Any())
                throw new SheaftException(ExceptionKind.NotFound);

            return items;
        }

        public async Task<IEnumerable<T>> FindAsync<T>(Expression<Func<T, bool>> where, CancellationToken token) where T : class, ITrackRemove
        {
            var items = await Set<T>().Where(c => !c.RemovedOn.HasValue).Where(where).ToListAsync(token);
            if (items == null || !items.Any())
                return new List<T>();

            return items;
        }

        public async Task<T> GetSingleAsync<T>(Expression<Func<T, bool>> where, CancellationToken token) where T : class, ITrackRemove
        {
            var item = await Set<T>().Where(c => !c.RemovedOn.HasValue).Where(where).SingleOrDefaultAsync(token);
            if (item == null)
                throw new SheaftException(ExceptionKind.NotFound);

            return item;
        }

        public async Task<T> FindSingleAsync<T>(Expression<Func<T, bool>> where, CancellationToken token) where T : class, ITrackRemove
        {
            return await Set<T>().Where(c => !c.RemovedOn.HasValue).Where(where).SingleOrDefaultAsync(token);
        }

        public async Task<bool> AnyAsync<T>(Expression<Func<T, bool>> where, CancellationToken token) where T : class, ITrackRemove
        {
            return await Set<T>().Where(c => !c.RemovedOn.HasValue).AnyAsync(where, token);
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

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            UpdateRelatedData();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private KeyValuePair<MessageKind, string> GetExceptionDefaultMessage<T>(MessageKind kind, Guid id) where T : class, IIdEntity, ITrackRemove
        {
            var messageKind = kind;
            var resource = id.ToString("N");

            if (Enum.TryParse(typeof(MessageKind), $"{nameof(T)}_{kind}", out object parsedKind))
                messageKind = (MessageKind)parsedKind;
            else 
                resource = $"{_localizer[typeof(T).Name, string.Empty]} ({id})";            

            return new KeyValuePair<MessageKind, string>(messageKind, resource);
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
        }
    }
}