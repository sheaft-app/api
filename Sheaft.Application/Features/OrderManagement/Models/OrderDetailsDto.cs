using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.OrderManagement;

public record OrderDetailsDto(string Id, string Code, OrderStatus Status, decimal TotalWholeSalePrice,
    decimal TotalOnSalePrice, decimal TotalVatPrice, DateTimeOffset CreatedOn, DateTimeOffset UpdatedOn,
    DateTimeOffset? PublishedOn, DateTimeOffset? AcceptedOn, DateTimeOffset? CompletedOn, DateTimeOffset? FulfilledOn,
    OrderUserDto Supplier, OrderUserDto Customer, IEnumerable<OrderLineDto> Lines, OrderDeliveryDto Delivery);

public record OrderDeliveryDto(string Id, DateTimeOffset ScheduledAt, DeliveryStatus Status, NamedAddressDto Address);