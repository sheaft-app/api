using Sheaft.Application.Models;
using Sheaft.Domain;

namespace Sheaft.Application.OrderManagement;

public record OrderDeliveryDto(string Id, DateTimeOffset ScheduledAt, DeliveryStatus Status, NamedAddressDto Address);