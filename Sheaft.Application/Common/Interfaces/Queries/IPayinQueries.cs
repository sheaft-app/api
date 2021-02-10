using System;
using System.Linq;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Domain;

namespace Sheaft.Application.Common.Interfaces.Queries
{
    public interface IPayinQueries
    {
        IQueryable<WebPayinDto> GetWebPayinTransaction(Guid id, RequestUser currentUser);
        IQueryable<WebPayinDto> GetWebPayinTransaction(string identifier, RequestUser currentUser);
    }
}