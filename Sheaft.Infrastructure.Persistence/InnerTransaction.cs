using System;
using System.Threading;
using System.Threading.Tasks;
using Sheaft.Domain.Models;
using Sheaft.Domain.Views;
using Microsoft.EntityFrameworkCore;
using Sheaft.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Extensions.Localization;
using Sheaft.Localization;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Sheaft.Application.Interop;
using Sheaft.Domain.Interop;
using Microsoft.EntityFrameworkCore.Storage;

namespace Sheaft.Infrastructure.Persistence
{

    public class InnerTransaction : IDbContextTransaction
    {
        public InnerTransaction(Guid transactionId)
        {
            TransactionId = transactionId;
        }

        public Guid TransactionId { get; }

        public void Commit()
        {
        }

        public Task CommitAsync(CancellationToken token = default)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
        }

        public ValueTask DisposeAsync()
        {
            return new ValueTask(Task.CompletedTask);
        }

        public void Rollback()
        {
        }

        public Task RollbackAsync(CancellationToken token = default)
        {
            return Task.CompletedTask;
        }
    }
}