using Sheaft.Domain;

namespace Sheaft.Infrastructure;

internal abstract class Queries
{
    protected async Task<Result<T>> QueryAsync<T>(Func<Task<Result<T>>> func)
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

    protected async Task<Result<Maybe<T>>> QueryAsync<T>(Func<Task<Result<Maybe<T>>>> func)
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

    protected async Task<Result<IEnumerable<T>>> QueryAsync<T>(Func<Task<Result<IEnumerable<T>>>> func)
    {
        try
        {
            return await func();
        }
        catch (Exception e)
        {
            return Result.Failure<IEnumerable<T>>(ErrorKind.Unexpected, "database.error", e.Message);
        }
    }
}