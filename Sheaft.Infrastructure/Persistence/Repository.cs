using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Mediator;
using Sheaft.Application.Persistence;
using Sheaft.Domain;
using Sheaft.Domain.Common;
using Sheaft.Domain.Events;
using Sheaft.Domain.Interop;
using Sheaft.Infrastructure.Persistence.Database;

namespace Sheaft.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IMediator _mediatr;
        private readonly DbContext _context;

        public UnitOfWork(
            IMediator mediatr,
            IAppDbContext context)
        {
            _mediatr = mediatr;
            _context = context as DbContext;
        }
        
        public async Task<Result<int>> SaveChanges(CancellationToken token)
        {
            try
            {
                UpdateRelatedData();
                return Result<int>.Success(await _context.SaveChangesAsync(token));
            }
            catch (Exception e)
            {
                return Result<int>.Failure(e);
            }
        }

        public async Task<ITransaction> BeginTransaction(CancellationToken token = default)
        {
            if (_context.Database.CurrentTransaction != null)
                return new InnerTransaction();

            return new InnerTransaction(await _context.Database.BeginTransactionAsync(token), DispatchEvents);
        }

        private void UpdateRelatedData()
        {
            _context.ChangeTracker.DetectChanges();

            foreach (var entry in _context.ChangeTracker.Entries())
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
            
            if(_context.Database.CurrentTransaction == null)
                DispatchEvents();
        }

        private void DispatchEvents()
        {
            while (true)
            {
                var domainEventEntity = _context.ChangeTracker.Entries<IHasDomainEvent>()
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
    
    public abstract class Repository<T> : IRepository<T> where T: class, IIdEntity
    {
        protected DbSet<T> DbSet { get; init; }
        
        public virtual async Task<Result<T>> GetById(Guid id, CancellationToken token)
        {
            try
            {
                var entity = await DbSet.SingleOrDefaultAsync(c => c.Id == id, token);
                if(entity == null)
                    return Result<T>.Failure($"L'identifiant {id} est introuvable.");
                    
                return Result<T>.Success(entity);
            }
            catch (Exception e)
            {
                return Result<T>.Failure(e);
            }
        }
        
        public async Task<Result<Guid>> Add(T entity, CancellationToken token)
        {
            try
            {
                await DbSet.AddAsync(entity, token);
                return Result<Guid>.Success(entity.Id);
            }
            catch (Exception e)
            {
                return Result<Guid>.Failure(e);
            }
        }

        public async Task<Result> Remove(Guid id)
        {
            try
            {
                var entity = await DbSet.SingleAsync(e => e.Id == id);
                DbSet.Remove(entity);
                return Result.Success();
            }
            catch(Exception e)
            {
                return Result.Failure(e);
            }
        }
    }

    public class CompanyRepository : Repository<Company>
    {
        public CompanyRepository(IAppDbContext context)
        {
            DbSet = ((DbContext)context).Set<Company>();
        }
    }
}