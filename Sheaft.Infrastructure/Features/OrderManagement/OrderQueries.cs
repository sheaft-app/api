using DataModel;
using LinqToDB;
using Sheaft.Application;
using Sheaft.Application.Models;
using Sheaft.Application.OrderManagement;
using Sheaft.Domain;

namespace Sheaft.Infrastructure.OrderManagement;

internal class OrderQueries : Queries, IOrderQueries
{
    private readonly AppDb _context;

    public OrderQueries(AppDb context)
    {
        _context = context;
    }

    public Task<Result<OrderDetailsDto>> Get(OrderId identifier, AccountId accountId, CancellationToken token)
    {
        return QueryAsync(async () =>
        {
            var ordersQuery =
                from order in _context.Orders
                where order.Id == identifier.Value
                join delivery in _context.Deliveries on order.DeliveryId equals delivery.Id
                join orderLine in _context.OrderLines on order.Id equals orderLine.OrderId
                    into orderLines
                from dl in _context.Deliveries.InnerJoin(d => d.Id == order.DeliveryId)
                join deliveryLine in _context.DeliveryLines on dl.Id equals deliveryLine.DeliveryId
                    into deliveryLines
                from da in _context.Deliveries.InnerJoin(d => d.Id == order.DeliveryId)
                join deliveryAdjustment in _context.DeliveryAdjustments on da.Id equals deliveryAdjustment.DeliveryId
                    into deliveryAdjustments
                select new OrderDetailsDto(
                    order.Id,
                    order.Reference,
                    (OrderStatus) order.Status,
                    order.TotalWholeSalePrice,
                    order.TotalOnSalePrice,
                    order.TotalVatPrice,
                    order.CreatedOn,
                    order.UpdatedOn,
                    order.PublishedOn,
                    order.AcceptedOn,
                    (OrderStatus) order.Status == OrderStatus.Completed ? order.CompletedOn : null,
                    (OrderStatus) order.Status == OrderStatus.Refused ||
                    (OrderStatus) order.Status == OrderStatus.Cancelled
                        ? order.CompletedOn
                        : null,
                    order.FulfilledOn,
                    new OrderUserDto(order.SupplierId, order.Supplier.TradeName, order.Supplier.Email, order.Supplier.Phone),
                    new OrderUserDto(order.CustomerId, order.Customer.TradeName, order.Customer.Email, order.Customer.Phone),
                    orderLines.Select(l =>
                        new OrderLineDto(
                            (OrderLineKind) l.LineKind,
                            l.Identifier,
                            l.Name,
                            l.Reference,
                            l.Quantity,
                            l.Vat,
                            l.PriceInfoUnitPrice,
                            l.PriceInfoWholeSalePrice,
                            l.PriceInfoVatPrice,
                            l.PriceInfoOnSalePrice)),
                    delivery != null
                        ? new OrderDeliveryDto(delivery.Id, delivery.ScheduledAt,
                            (DeliveryStatus) delivery.Status,
                            delivery.TotalWholeSalePrice,
                            delivery.TotalOnSalePrice,
                            delivery.TotalVatPrice,
                            deliveryLines.Select(l => new DeliveryLineDto(
                                (DeliveryLineKind) l.LineKind,
                                l.Identifier,
                                l.Name,
                                l.Reference,
                                l.Quantity,
                                l.Vat,
                                l.PriceInfoUnitPrice,
                                l.PriceInfoWholeSalePrice,
                                l.PriceInfoVatPrice,
                                l.PriceInfoOnSalePrice)),
                            deliveryAdjustments.Select(l => new DeliveryLineDto(
                                (DeliveryLineKind) l.LineKind,
                                l.Identifier,
                                l.Name,
                                l.Reference,
                                l.Quantity,
                                l.Vat,
                                l.PriceInfoUnitPrice,
                                l.PriceInfoWholeSalePrice,
                                l.PriceInfoVatPrice,
                                l.PriceInfoOnSalePrice)),
                            new NamedAddressDto(delivery.AddressName, delivery.AddressEmail,
                                delivery.AddressStreet, delivery.AddressComplement,
                                delivery.AddressPostcode, delivery.AddressCity), delivery.Comments)
                        : null,
                    accountId.Value == order.Supplier.AccountId && (OrderStatus) order.Status == OrderStatus.Pending,
                    (accountId.Value == order.Customer.AccountId && (OrderStatus) order.Status == OrderStatus.Pending) ||
                    (accountId.Value == order.Supplier.AccountId &&
                     ((OrderStatus) order.Status == OrderStatus.Accepted ||
                      (OrderStatus) order.Status == OrderStatus.Fulfilled)),
                    accountId.Value == order.Supplier.AccountId && (OrderStatus) order.Status == OrderStatus.Accepted,
                    accountId.Value == order.Supplier.AccountId && (OrderStatus) order.Status == OrderStatus.Fulfilled,
                    order.FailureReason);

            var orderResult = await ordersQuery.FirstOrDefaultAsync(token);
            
            return orderResult == null
                ? Result.Failure<OrderDetailsDto>(ErrorKind.NotFound, "order.notfound")
                : Result.Success(orderResult);
        });
    }

    public Task<Result<OrderDraftDto>> GetDraft(OrderId identifier, AccountId accountId, CancellationToken token)
    {
        return QueryAsync(async () =>
        {
            var ordersQuery =
                from order in _context.Orders
                from line in _context.OrderLines.LeftJoin(ol => ol.OrderId == order.Id)
                from product in _context.Products.LeftJoin(p => p.Id == line.Identifier)
                from catalogProduct in _context.CatalogProducts.LeftJoin(cp => cp.ProductId == product.Id)
                from returnable in _context.Returnables.LeftJoin(r => r.Id == line.Identifier)
                where order.Id == identifier.Value
                group new
                {
                    line.LineKind, line.Identifier, line.Quantity, ProductName = product.Name,
                    ProductReference = product.Reference, ProductVat = product.Vat,
                    ProductUnitPrice = catalogProduct.UnitPrice,
                    ReturnableVat = returnable.Vat, ReturnableUnitPrice = returnable.UnitPrice,
                    ReturnableName = returnable.Name, ReturnableReference = returnable.Reference
                } by new
                {
                    order.Id,
                    order.CreatedOn,
                    order.UpdatedOn,
                    SupplierId = order.Supplier.Id,
                    SupplierName = order.Supplier.TradeName,
                    SupplierEmail = order.Supplier.Email,
                    SupplierPhone = order.Supplier.Phone,
                    CustomerId = order.Customer.Id,
                    CustomerName = order.Customer.TradeName,
                    CustomerEmail = order.Customer.Email,
                    CustomerPhone = order.Customer.Phone,
                }
                into g
                select new OrderDraftDto(
                    g.Key.Id,
                    g.Key.CreatedOn,
                    g.Key.UpdatedOn,
                    new OrderUserDto(g.Key.SupplierId, g.Key.SupplierName, g.Key.SupplierEmail, g.Key.SupplierPhone),
                    new OrderUserDto(g.Key.CustomerId, g.Key.CustomerName, g.Key.CustomerEmail, g.Key.CustomerPhone),
                    g.Select(l => (OrderLineKind) l.LineKind == OrderLineKind.Product
                        ? new OrderDraftLineDto((OrderLineKind) l.LineKind, l.Identifier, l.ProductName,
                            l.ProductReference, l.Quantity, l.ProductVat, l.ProductUnitPrice)
                        : new OrderDraftLineDto((OrderLineKind) l.LineKind, l.Identifier, l.ReturnableName,
                            l.ReturnableReference, l.Quantity, l.ReturnableVat, l.ReturnableUnitPrice)));

            var orderResult = await ordersQuery.FirstOrDefaultAsync(token);
            return orderResult == null
                ? Result.Failure<OrderDraftDto>(ErrorKind.NotFound, "order.draft.notfound")
                : Result.Success(orderResult);
        });
    }

    public Task<Result<PagedResult<OrderDto>>> List(AccountId accountId, IEnumerable<OrderStatus> statuses,
        PageInfo pageInfo, CancellationToken token)
    {
        return QueryAsync(async () =>
        {
            var ordersQuery =
                from order in _context.Orders
                from delivery in _context.Deliveries.LeftJoin(d => d.Id == order.DeliveryId)
                where (order.Customer.AccountId == accountId.Value || order.Supplier.AccountId == accountId.Value)
                      && statuses.Contains((OrderStatus) order.Status)
                orderby order.Reference
                select new
                {
                    order.Id,
                    order.Reference,
                    order.Status,
                    TotalWholeSalePrice = delivery.Id != null && (DeliveryStatus)delivery.Status != DeliveryStatus.Pending ? delivery.TotalWholeSalePrice : order.TotalWholeSalePrice,
                    TotalOnSalePrice = delivery.Id != null && (DeliveryStatus)delivery.Status != DeliveryStatus.Pending ? delivery.TotalOnSalePrice : order.TotalOnSalePrice,
                    TotalVatPrice = delivery.Id != null && (DeliveryStatus)delivery.Status != DeliveryStatus.Pending ? delivery.TotalVatPrice : order.TotalVatPrice,
                    order.CreatedOn,
                    order.UpdatedOn,
                    order.PublishedOn,
                    order.AcceptedOn,
                    CompletedOn = (OrderStatus) order.Status == OrderStatus.Completed ? order.CompletedOn : null,
                    AbortedOn = (OrderStatus) order.Status == OrderStatus.Refused ||
                                (OrderStatus) order.Status == OrderStatus.Cancelled
                        ? order.CompletedOn
                        : null,
                    order.FulfilledOn,
                    DeliveryScheduledAt = delivery.Id != null ? delivery.ScheduledAt : (DateTimeOffset?) null,
                    DeliveryStatus = delivery.Id != null ? delivery.Status : (int?) null,
                    CustomerName = order.Customer.TradeName,
                    SupplierName = order.Supplier.TradeName,
                    TotalCount = Sql.Ext.Count().Over().ToValue()
                };

            var orders = (await ordersQuery
                    .Skip(pageInfo.Skip)
                    .Take(pageInfo.Take)
                    .ToListAsync(token))
                .GroupBy(p => p.TotalCount)
                .FirstOrDefault();

            return Result.Success(new PagedResult<OrderDto>(
                orders?.Select(o =>
                    new OrderDto(o.Id, o.Reference, (OrderStatus) o.Status, o.TotalWholeSalePrice, o.TotalOnSalePrice,
                        o.TotalVatPrice, o.CreatedOn, o.UpdatedOn, o.PublishedOn, o.AcceptedOn, o.CompletedOn,
                        o.AbortedOn,
                        o.FulfilledOn, o.DeliveryStatus.HasValue ? (DeliveryStatus) o.DeliveryStatus.Value : null,
                        o.DeliveryScheduledAt, o.CustomerName, o.SupplierName)),
                pageInfo, orders?.Key ?? 0));
        });
    }
}