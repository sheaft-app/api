using Sheaft.Models.Dto;
using Sheaft.Interop;
using System;
using System.Linq;

namespace Sheaft.Application.Queries
{
    public interface IPurchaseOrderQueries
    {
        IQueryable<PurchaseOrderDto> GetPurchaseOrder(Guid id, IRequestUser currentUser);
        IQueryable<PurchaseOrderDto> GetPurchaseOrders(IRequestUser currentUser);
        IQueryable<PurchaseOrderDto> MyPurchaseOrders(IRequestUser currentUser);
    }
}