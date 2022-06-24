using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.OrderManagement;

public record OrderDeliveryDto(string Id, DateTimeOffset ScheduledAt, DeliveryStatus Status, decimal TotalWholeSalePrice,
    decimal TotalOnSalePrice, decimal TotalVatPrice, IEnumerable<DeliveryLineDto> Lines, IEnumerable<DeliveryLineDto> Adjustments, NamedAddressDto Address, string Comments);