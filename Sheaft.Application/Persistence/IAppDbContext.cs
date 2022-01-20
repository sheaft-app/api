using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Application.Persistence
{
    public interface IAppDbContext
    {
    }
    
    public interface ITransaction : IDisposable, IAsyncDisposable
    {
        Guid TransactionId { get; }
        Task CommitAsync(CancellationToken cancellationToken = default);
        Task RollbackAsync(CancellationToken cancellationToken = default);
    }
}