using Sheaft.Domain;

namespace Sheaft.Application.OrderManagement;

public record OrderDetailsDto(string Id, string Code, OrderStatus Status, decimal TotalWholeSalePrice,
    decimal TotalOnSalePrice, decimal TotalVatPrice, DateTimeOffset CreatedOn, DateTimeOffset UpdatedOn,
    DateTimeOffset? PublishedOn, DateTimeOffset? AcceptedOn, DateTimeOffset? CompletedOn, DateTimeOffset? AbortedOn, DateTimeOffset? FulfilledOn,
    OrderUserDto Supplier, OrderUserDto Customer, IEnumerable<OrderLineDto> Lines, OrderDeliveryDto Delivery, bool CanAcceptOrRefuse, bool CanCancel, bool CanFulfill, bool CanComplete);