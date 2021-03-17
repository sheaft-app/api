using System;
using System.Linq;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Domain;

namespace Sheaft.Application.Common.Interfaces.Queries
{
    public interface IQuickOrderQueries
    {
        IQueryable<QuickOrderDto> GetQuickOrder(Guid quickOrderId, RequestUser currentUser);
        IQueryable<QuickOrderDto> GetQuickOrders(RequestUser currentUser);
        IQueryable<QuickOrderDto> GetUserDefaultQuickOrder(Guid userId, RequestUser currentUser);
    }
}