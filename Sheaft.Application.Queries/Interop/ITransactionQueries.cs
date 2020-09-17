using Sheaft.Core;
using Sheaft.Models.Dto;
using System;
using System.Linq;

namespace Sheaft.Application.Queries
{
    public interface ITransactionQueries
    {
        IQueryable<WebPayinTransactionDto> GetWebPayinTransaction(Guid id, RequestUser currentUser);
        IQueryable<WebPayinTransactionDto> GetWebPayinTransaction(string identifier, RequestUser currentUser);
    }
}