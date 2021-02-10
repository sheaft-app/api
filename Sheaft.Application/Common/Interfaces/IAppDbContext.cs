﻿using System;
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
using Sheaft.Domain;
using Sheaft.Domain.Interop;
using Sheaft.Domain.Views;

namespace Sheaft.Application.Common.Interfaces
{
    public interface IAppDbContext : IDisposable, IAsyncDisposable, IInfrastructure<IServiceProvider>, IDbContextDependencies, IDbSetCache, IDbContextPoolable, IResettableService
    {
        DbSet<Domain.Agreement> Agreements { get; set; }
        DbSet<BankAccount> BankAccounts { get; set; }
        DbSet<Domain.Country> Countries { get; set; }
        DbSet<Domain.DeliveryMode> DeliveryModes { get; set; }
        DbSet<Domain.Department> Departments { get; set; }
        DbSet<Domain.Donation> Donations { get; set; }
        DbSet<Domain.Job> Jobs { get; set; }
        DbSet<Domain.Legal> Legals { get; set; }
        DbSet<Domain.Level> Levels { get; set; }
        DbSet<Domain.Nationality> Nationalities { get; set; }
        DbSet<Domain.Notification> Notifications { get; set; }
        DbSet<Domain.Order> Orders { get; set; }
        DbSet<Domain.PurchaseOrder> PurchaseOrders { get; set; }
        DbSet<Domain.Returnable> Returnables { get; set; }
        DbSet<Domain.Payin> Payins { get; set; }
        DbSet<Domain.Payout> Payouts { get; set; }
        DbSet<Domain.Product> Products { get; set; }
        DbSet<Domain.QuickOrder> QuickOrders { get; set; }
        DbSet<Refund> Refunds { get; set; }
        DbSet<Domain.Region> Regions { get; set; }
        DbSet<Domain.Reward> Rewards { get; set; }
        DbSet<Domain.Tag> Tags { get; set; }
        DbSet<Domain.Transfer> Transfers { get; set; }
        DbSet<Domain.User> Users { get; set; }
        DbSet<Domain.Wallet> Wallets { get; set; }
        DbSet<Domain.Withholding> Withholdings { get; set; }

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
        EntityEntry<TEntity> Add<TEntity>([NotNull] TEntity entity) where TEntity : class;
        ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>([NotNull] TEntity entity, CancellationToken cancellationToken = default) where TEntity : class;
        Task AddRangeAsync([NotNull] IEnumerable<object> entities, CancellationToken cancellationToken = default);
        EntityEntry<TEntity> Remove<TEntity>([NotNull] TEntity entity) where TEntity : class;
        void RemoveRange([NotNull] IEnumerable<object> entities);
        int SaveChanges(bool acceptAllChangesOnSuccess);
        int SaveChanges();
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        EntityEntry<TEntity> Update<TEntity>([NotNull] TEntity entity) where TEntity : class;
        void UpdateRange([NotNull] IEnumerable<object> entities);
        EntityEntry<TEntity> Restore<TEntity>([NotNull] TEntity entity)
            where TEntity : class, ITrackRemove;
    }
}
