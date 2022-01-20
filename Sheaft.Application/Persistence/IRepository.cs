using System;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Domain.Common;
using Sheaft.Domain.Interop;

namespace Sheaft.Application.Persistence
{
    public interface IRepository<T>where T: class, IIdEntity
    {
        Task<Result<T>> GetById(Guid id, CancellationToken token = default);
        Task<Result<Guid>> Add(T entity, CancellationToken token = default);
        Task<Result> Remove(Guid id);
    }

    public interface IUnitOfWork
    {
        Task<Result<int>> SaveChanges(CancellationToken token = default);
        Task<ITransaction> BeginTransaction(CancellationToken token = default);
    }
}