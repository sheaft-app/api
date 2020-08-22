using Sheaft.Models.Dto;
using Sheaft.Core;
using System;
using System.Linq;

namespace Sheaft.Application.Queries
{
    public interface IPurchaseOrderQueries
    {
        IQueryable<PurchaseOrderDto> GetPurchaseOrder(Guid id, RequestUser currentUser);
        IQueryable<PurchaseOrderDto> GetPurchaseOrders(RequestUser currentUser);
        IQueryable<PurchaseOrderDto> MyPurchaseOrders(RequestUser currentUser);
    }
}