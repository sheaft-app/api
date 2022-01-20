using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using Sheaft.Application.Persistence;

namespace Sheaft.Infrastructure.Persistence.Database
{
    internal class InnerTransaction : ITransaction
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
            return TransactionId == Guid.Empty ? new ValueTask(Task.CompletedTask) : _transaction.DisposeAsync();
        }

        public Task RollbackAsync(CancellationToken token = default)
        {
            return TransactionId == Guid.Empty ? Task.CompletedTask : _transaction.RollbackAsync(token);
        }
    }
}