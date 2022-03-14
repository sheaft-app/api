namespace Sheaft.Domain;

public interface IRepository<T, in TU>
    where T: class, IEntity
{
    Task<Result<T>> Get(TU identifier, CancellationToken token);
}