using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace Sheaft.Infrastructure.Persistence.Database
{
    internal class InnerTransaction : IDbContextTransaction
    {
        private readonly IDbContextTransaction _transaction;
        private readonly Action _action;

        public InnerTransaction()
        {
            TransactionId = Guid.Empty;
        }

        public InnerTransaction(IDbContextTransaction transaction, Action action)
        {
            _transaction = transaction;
            _action = action;
            TransactionId = transaction.TransactionId;
        }

        public Guid TransactionId { get; }

        public void Commit()
        {
            if(TransactionId == Guid.Empty)
                return;
            
            _action();
            _transaction.Commit();
        }

        public Task CommitAsync(CancellationToken token = default)
        {
            if(TransactionId == Guid.Empty)
                return Task.CompletedTask;
                
            _action();
            return _transaction.CommitAsync(token);
        }

        public void Dispose()
        {
            if(TransactionId == Guid.Empty)
                return;
            
            _transaction.Dispose();
        }

        public ValueTask DisposeAsync()
        {
            if(TransactionId == Guid.Empty)
                return new ValueTask(Task.CompletedTask);
                
            return _transaction.DisposeAsync();
        }

        public void Rollback()
        {
            if(TransactionId == Guid.Empty)
                return;

            _transaction.Rollback();
        }

        public Task RollbackAsync(CancellationToken token = default)
        {
            if(TransactionId == Guid.Empty)
                return Task.CompletedTask;

            return _transaction.RollbackAsync(token);
        }
    }
}