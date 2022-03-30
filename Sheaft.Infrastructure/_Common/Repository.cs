using Microsoft.EntityFrameworkCore;
using Sheaft.Domain;
using Sheaft.Infrastructure.Persistence;

namespace Sheaft.Infrastructure;

internal abstract class Repository<T, TU> : IRepository<T, TU>
    where T : class, IAggregateRoot
{
    protected readonly DbSet<T> Values;

    protected Repository(IDbContext context)
    {
        Values = context.Set<T>();
    }

    protected async Task<Result<T>> QueryAsync(Func<Task<Result<T>>> func)
    {
        try
        {
            return await func();
        }
        catch (Exception e)
        {
            return Result.Failure<T>(ErrorKind.Unexpected, "database.error", e.Message);
        }
    }

    protected async Task<Result<Maybe<T>>> QueryAsync(Func<Task<Result<Maybe<T>>>> func)
    {
        try
        {
            return await func();
        }
        catch (Exception e)
        {
            return Result.Failure<Maybe<T>>(ErrorKind.Unexpected, "database.error", e.Message);
        }
    }
    
    public void Add(T entity)
    {
        Values.Add(entity);
    }

    public void Update(T entity)
    {
        //Values.Update(entity);
    }

    public void Remove(T entity)
    {
        Values.Remove(entity);
    }

    public abstract Task<Result<T>> Get(TU identifier, CancellationToken token);
}