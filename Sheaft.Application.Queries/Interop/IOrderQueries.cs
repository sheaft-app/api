using Sheaft.Core;
using Sheaft.Models.Dto;
using System;
using System.Linq;

namespace Sheaft.Application.Queries
{
    public interface IOrderQueries
    {
        IQueryable<OrderDto> GetOrder(Guid id, RequestUser currentUser);
        IQueryable<OrderDto> GetOrders(RequestUser currentUser);
    }
}