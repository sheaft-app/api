using Sheaft.Core;
using Sheaft.Models.Dto;
using System;
using System.Linq;

namespace Sheaft.Application.Queries
{
    public interface ITransactionQueries
    {
        IQueryable<T> GetTransactions<T>(RequestUser currentUser) where T : BaseTransactionDto;
        IQueryable<T> GetTransaction<T>(Guid id, RequestUser currentUser) where T : BaseTransactionDto;
        IQueryable<T> GetTransaction<T>(string identifier, RequestUser currentUser) where T : BaseTransactionDto;
    }
}