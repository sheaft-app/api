using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Sheaft.Domain.Interop;
using Sheaft.Domain.Models;
using Sheaft.Domain.Views;

namespace Sheaft.Application.Interop
{
    public interface IAppDbContext : IDisposable, IAsyncDisposable, IInfrastructure<IServiceProvider>, IDbContextDependencies, IDbSetCache, IDbContextPoolable, IResettableService
    {
        DbSet<Agreement> Agreements { get; set; }
        DbSet<BankAccount> BankAccounts { get; set; }
        DbSet<Country> Countries { get; set; }
        DbSet<DeliveryMode> DeliveryModes { get; set; }
        DbSet<Department> Departments { get; set; }
        DbSet<Donation> Donations { get; set; }
        DbSet<Job> Jobs { get; set; }
        DbSet<Legal> Legals { get; set; }
        DbSet<Level> Levels { get; set; }
        DbSet<Nationality> Nationalities { get; set; }
        DbSet<Notification> Notifications { get; set; }
        DbSet<Order> Orders { get; set; }
        DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        DbSet<Returnable> Returnables { get; set; }
        DbSet<Payin> Payins { get; set; }
        DbSet<Payout> Payouts { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<QuickOrder> QuickOrders { get; set; }
        DbSet<Refund> Refunds { get; set; }
        DbSet<Region> Regions { get; set; }
        DbSet<Reward> Rewards { get; set; }
        DbSet<Tag> Tags { get; set; }
        DbSet<Transfer> Transfers { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<Wallet> Wallets { get; set; }
        DbSet<Withholding> Withholdings { get; set; }

        DbSet<CountryPoints> CountryPoints { get; set; }
        DbSet<CountryUserPoints> CountryUserPoints { get; set; }
        DbSet<RegionPoints> RegionPoints { get; set; }
        DbSet<RegionUserPoints> RegionUserPoints { get; set; }
        DbSet<DepartmentPoints> DepartmentPoints { get; set; }
        DbSet<DepartmentUserPoints> DepartmentUserPoints { get; set; }
        DbSet<DepartmentProducers> DepartmentProducers { get; set; }
        DbSet<DepartmentStores> DepartmentStores { get; set; }

        Task<T> GetByIdAsync<T>(Guid id, CancellationToken token, bool asNoTracking = false) where T : class, IIdEntity, ITrackRemove;
        Task<T> FindByIdAsync<T>(Guid id, CancellationToken token, bool asNoTracking = false) where T : class, IIdEntity, ITrackRemove;
        Task<T> GetSingleAsync<T>(Expression<Func<T, bool>> where, CancellationToken token, bool asNoTracking = false) where T : class, ITrackRemove;
        Task<T> FindSingleAsync<T>(Expression<Func<T, bool>> where, CancellationToken token, bool asNoTracking = false) where T : class, ITrackRemove;
        Task<IEnumerable<T>> GetByIdsAsync<T>(IEnumerable<Guid> ids, CancellationToken token, bool asNoTracking = false) where T : class, IIdEntity, ITrackRemove;
        Task<IEnumerable<T>> FindByIdsAsync<T>(IEnumerable<Guid> ids, CancellationToken token, bool asNoTracking = false) where T : class, IIdEntity, ITrackRemove;
        Task<IEnumerable<T>> GetAsync<T>(Expression<Func<T, bool>> where, CancellationToken token, bool asNoTracking = false) where T : class, ITrackRemove;
        Task<IEnumerable<T>> GetAsync<T>(CancellationToken token, bool asNoTracking = false) where T : class, ITrackRemove;
        Task<IEnumerable<T>> FindAsync<T>(Expression<Func<T, bool>> where, CancellationToken token, bool asNoTracking = false) where T : class, ITrackRemove;
        Task<IEnumerable<T>> FindAsync<T>(CancellationToken token, bool asNoTracking = false) where T : class, ITrackRemove;
        Task<bool> AnyAsync<T>(Expression<Func<T, bool>> where, CancellationToken token, bool asNoTracking = false) where T : class, ITrackRemove;
        Task<bool> AnyAsync<T>(CancellationToken token, bool asNoTracking = false) where T : class, ITrackRemove;
        Task EnsureNotExists<T>(Guid id, CancellationToken token, bool asNoTracking = false) where T : class, IIdEntity, ITrackRemove;
        Task EnsureNotExists<T>(Expression<Func<T, bool>> where, CancellationToken token, bool asNoTracking = false) where T : class, IIdEntity, ITrackRemove;
        
        void Migrate();
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
        EntityEntry<TEntity> Add<TEntity>([NotNullAttribute] TEntity entity) where TEntity : class;
        ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>([NotNullAttribute] TEntity entity, CancellationToken cancellationToken = default) where TEntity : class;
        Task AddRangeAsync([NotNullAttribute] IEnumerable<object> entities, CancellationToken cancellationToken = default);
        EntityEntry<TEntity> Remove<TEntity>([NotNullAttribute] TEntity entity) where TEntity : class;
        void RemoveRange([NotNullAttribute] IEnumerable<object> entities);
        int SaveChanges(bool acceptAllChangesOnSuccess);
        int SaveChanges();
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        EntityEntry<TEntity> Update<TEntity>([NotNullAttribute] TEntity entity) where TEntity : class;
        void UpdateRange([NotNullAttribute] IEnumerable<object> entities);
        EntityEntry<TEntity> Restore<TEntity>([NotNullAttribute] TEntity entity)
            where TEntity : class, ITrackRemove;
    }
}
