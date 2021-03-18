using System;
using System.Linq;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Interfaces.Queries
{
    public interface IQuickOrderQueries
    {
        IQueryable<QuickOrderDto> GetQuickOrder(Guid quickOrderId, RequestUser currentUser);
        IQueryable<QuickOrderDto> GetQuickOrders(RequestUser currentUser);
        IQueryable<QuickOrderDto> GetUserDefaultQuickOrder(Guid userId, RequestUser currentUser);
    }
}