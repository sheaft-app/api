using System;
using System.Linq;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Interfaces.Queries
{
    public interface IPayinQueries
    {
        IQueryable<PayinDto> GetPayin(Guid id, RequestUser currentUser);
        IQueryable<PayinDto> GetPayin(string identifier, RequestUser currentUser);
        IQueryable<WebPayinDto> GetWebPayin(Guid id, RequestUser currentUser);
        IQueryable<PayinDto> GetPreAuthorizedPayin(Guid id, RequestUser currentUser);
    }
}