using Sheaft.Core;
using Sheaft.Models.Dto;
using System;
using System.Linq;

namespace Sheaft.Application.Queries
{
    public interface ITransactionQueries
    {
        IQueryable<TransactionDto> GetTransactions(RequestUser currentUser);
        IQueryable<T> GetTransactions<T>(RequestUser currentUser) where T : BaseTransactionDto;
        IQueryable<TransactionDto> GetTransaction(Guid id, RequestUser currentUser);
    }
}