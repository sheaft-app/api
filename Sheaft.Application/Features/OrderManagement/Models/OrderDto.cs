using Sheaft.Domain;

namespace Sheaft.Application.OrderManagement;

public record OrderDto(string Id, string Code, OrderStatus Status, decimal TotalWholeSalePrice, decimal TotalOnSalePrice, decimal TotalVatPrice, DateTimeOffset CreatedOn, DateTimeOffset UpdatedOn, 
    DateTimeOffset? PublishedOn, DateTimeOffset? AcceptedOn, DateTimeOffset? CompletedOn, DateTimeOffset? AbortedOn, DateTimeOffset? FulfilledOn, DeliveryStatus? DeliveryStatus, DateTimeOffset? DeliveryScheduledAt, string CustomerName, string SupplierName, string? DeliveryId, string? InvoiceId);