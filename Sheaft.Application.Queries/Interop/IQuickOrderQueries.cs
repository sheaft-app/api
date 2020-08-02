using Sheaft.Models.Dto;
using Sheaft.Interop;
using System;
using System.Linq;

namespace Sheaft.Application.Queries
{
    public interface IQuickOrderQueries
    {
        IQueryable<QuickOrderDto> GetQuickOrder(Guid quickOrderId, IRequestUser currentUser);
        IQueryable<QuickOrderDto> GetQuickOrders(IRequestUser currentUser);
        IQueryable<QuickOrderDto> GetUserDefaultQuickOrder(Guid userId, IRequestUser currentUser);
    }
}