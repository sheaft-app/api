using System;
using System.Linq;
using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.Interfaces.Queries
{
    public interface IPurchaseOrderQueries
    {
        IQueryable<PurchaseOrderDto> GetPurchaseOrder(Guid id, RequestUser currentUser);
        IQueryable<PurchaseOrderDto> GetPurchaseOrders(RequestUser currentUser);
        IQueryable<PurchaseOrderDto> MyPurchaseOrders(RequestUser currentUser);
    }
}