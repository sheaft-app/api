using System;
using System.Linq;
using Sheaft.Application.Common.Models.Dto;
using Sheaft.Domain;

namespace Sheaft.Application.Common.Interfaces.Queries
{
    public interface IPurchaseOrderQueries
    {
        IQueryable<PurchaseOrderDto> GetPurchaseOrder(Guid id, RequestUser currentUser);
        IQueryable<PurchaseOrderDto> GetPurchaseOrders(RequestUser currentUser);
        IQueryable<PurchaseOrderDto> MyPurchaseOrders(RequestUser currentUser);
    }
}