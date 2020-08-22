using Sheaft.Models.Dto;
using Sheaft.Core;
using System;
using System.Linq;

namespace Sheaft.Application.Queries
{
    public interface IQuickOrderQueries
    {
        IQueryable<QuickOrderDto> GetQuickOrder(Guid quickOrderId, RequestUser currentUser);
        IQueryable<QuickOrderDto> GetQuickOrders(RequestUser currentUser);
        IQueryable<QuickOrderDto> GetUserDefaultQuickOrder(Guid userId, RequestUser currentUser);
    }
}