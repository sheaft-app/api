using System;
using System.Linq;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Interfaces.Queries
{
    public interface IPayinQueries
    {
        IQueryable<WebPayinDto> GetWebPayinTransaction(Guid id, RequestUser currentUser);
        IQueryable<WebPayinDto> GetWebPayinTransaction(string identifier, RequestUser currentUser);
    }
}