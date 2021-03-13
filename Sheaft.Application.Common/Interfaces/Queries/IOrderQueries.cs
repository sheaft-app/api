using System;
using System.Linq;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Domain;

namespace Sheaft.Application.Common.Interfaces.Queries
{
    public interface IOrderQueries
    {
        IQueryable<OrderDto> GetOrder(Guid id, RequestUser currentUser);
        IQueryable<OrderDto> GetOrders(RequestUser currentUser);
        IQueryable<OrderDto> GetCurrentOrder(RequestUser currentUser);
    }
}