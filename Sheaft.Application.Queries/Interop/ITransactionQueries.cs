using Sheaft.Core;
using Sheaft.Application.Models;
using System;
using System.Linq;

namespace Sheaft.Application.Queries
{
    public interface ITransactionQueries
    {
        IQueryable<WebPayinDto> GetWebPayinTransaction(Guid id, RequestUser currentUser);
        IQueryable<WebPayinDto> GetWebPayinTransaction(string identifier, RequestUser currentUser);
    }
}