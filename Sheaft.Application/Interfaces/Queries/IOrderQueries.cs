using System;
using System.Linq;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Interfaces.Queries
{
    public interface IOrderQueries
    {
        IQueryable<OrderDto> GetOrder(Guid id, RequestUser currentUser);
        IQueryable<OrderDto> GetOrders(RequestUser currentUser);
        IQueryable<OrderDto> GetCurrentOrder(RequestUser currentUser);
    }
}