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
                from line in _context.OrderLines.InnerJoin(ol => ol.OrderId == order.Id)
                from delivery in _context.Deliveries.LeftJoin(d => d.Id == order.DeliveryId)
                where order.Id == identifier.Value
                group line by new
                {
                    order.Id,
                    order.Reference,
                    order.Status,
                    order.TotalWholeSalePrice,
                    order.TotalVatPrice,
                    order.TotalOnSalePrice,
                    order.CreatedOn,
                    order.UpdatedOn,
                    order.PublishedOn,
                    order.AcceptedOn,
                    order.CompletedOn,
                    order.FulfilledOn,
                    DeliveryId = delivery != null ? delivery.Id : null,
                    DeliveryScheduledAt = delivery != null ? delivery.ScheduledAt : (DateTimeOffset?) null,
                    DeliveryStatus = delivery != null ? delivery.Status : (int?) null,
                    DeliveryAddressName = delivery != null ? delivery.AddressName : null,
                    DeliveryAddressEmail = delivery != null ? delivery.AddressEmail : null,
                    DeliveryAddressStreet = delivery != null ? delivery.AddressStreet : null,
                    DeliveryAddressComplement = delivery != null ? delivery.AddressComplement : null,
                    DeliveryAddressPostCode = delivery != null ? delivery.AddressPostcode : null,
                    DeliveryAddressCity = delivery != null ? delivery.AddressCity : null,
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
                select new OrderDetailsDto(
                    g.Key.Id,
                    g.Key.Reference,
                    (OrderStatus) g.Key.Status,
                    g.Key.TotalWholeSalePrice,
                    g.Key.TotalOnSalePrice,
                    g.Key.TotalVatPrice,
                    g.Key.CreatedOn,
                    g.Key.UpdatedOn,
                    g.Key.PublishedOn,
                    g.Key.AcceptedOn,
                    g.Key.CompletedOn,
                    g.Key.FulfilledOn,
                    new OrderUserDto(g.Key.SupplierId, g.Key.SupplierName, g.Key.SupplierEmail, g.Key.SupplierPhone),
                    new OrderUserDto(g.Key.CustomerId, g.Key.CustomerName, g.Key.CustomerEmail, g.Key.CustomerPhone),
                    g.Select(l => new OrderLineDto((OrderLineKind) l.LineKind, l.Identifier, l.Name, l.Reference,
                        l.Quantity, l.Vat, l.PriceInfoUnitPrice, l.PriceInfoWholeSalePrice, l.PriceInfoVatPrice,
                        l.PriceInfoOnSalePrice)),
                    g.Key.DeliveryId != null
                        ? new OrderDeliveryDto(g.Key.DeliveryId, g.Key.DeliveryScheduledAt.Value,
                            (DeliveryStatus) g.Key.DeliveryStatus.Value,
                            new NamedAddressDto(g.Key.DeliveryAddressName, g.Key.DeliveryAddressEmail,
                                g.Key.DeliveryAddressStreet, g.Key.DeliveryAddressComplement,
                                g.Key.DeliveryAddressPostCode, g.Key.DeliveryAddressCity))
                        : null);

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
                from line in _context.OrderLines.InnerJoin(ol => ol.OrderId == order.Id)
                from product in _context.Products.LeftJoin(p => p.Id == line.Identifier)
                from catalogProduct in _context.CatalogProducts.LeftJoin(cp => cp.ProductId == product.Id)
                from returnable in _context.Returnables.LeftJoin(r => r.Id == line.Identifier)
                where order.Id == identifier.Value
                group new
                {
                    line.LineKind, line.Identifier, line.Quantity, ProductName = product.Name,
                    ProductReference = product.Reference, ProductVat = product.Vat, ProductUnitPrice = catalogProduct.UnitPrice,
                    ReturnableVat = returnable.Vat, ReturnableUnitPrice = returnable.UnitPrice,
                    ReturnableName = returnable.Name, ReturnableReference = returnable.Reference
                } by new
                {
                    order.Id,
                    order.Reference,
                    order.Status,
                    order.TotalWholeSalePrice,
                    order.TotalVatPrice,
                    order.TotalOnSalePrice,
                    order.CreatedOn,
                    order.UpdatedOn,
                    order.PublishedOn,
                    order.AcceptedOn,
                    order.CompletedOn,
                    order.FulfilledOn,
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
                        ? new OrderDraftLineDto((OrderLineKind) l.LineKind, l.Identifier, l.ProductName, l.ProductReference, l.Quantity, l.ProductVat, l.ProductUnitPrice)
                        : new OrderDraftLineDto((OrderLineKind) l.LineKind, l.Identifier, l.ReturnableName, l.ReturnableReference, l.Quantity, l.ReturnableVat, l.ReturnableUnitPrice)));

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
                orderby order.PublishedOn descending, order.CreatedOn descending
                select new
                {
                    order.Id,
                    order.Reference,
                    order.Status,
                    order.TotalWholeSalePrice,
                    order.TotalOnSalePrice,
                    order.TotalVatPrice,
                    order.CreatedOn,
                    order.UpdatedOn,
                    order.PublishedOn,
                    order.AcceptedOn,
                    order.CompletedOn,
                    order.FulfilledOn,
                    DeliveryScheduledAt = delivery != null ? delivery.ScheduledAt : (DateTimeOffset?) null,
                    DeliveryStatus = delivery != null ? delivery.Status : (int?) null,
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
                        o.FulfilledOn, o.DeliveryStatus.HasValue ? (DeliveryStatus) o.DeliveryStatus.Value : null,
                        o.DeliveryScheduledAt, o.CustomerName, o.SupplierName)),
                pageInfo, orders?.Key ?? 0));
        });
    }
}