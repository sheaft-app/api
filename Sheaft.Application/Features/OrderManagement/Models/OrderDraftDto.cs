namespace Sheaft.Application.OrderManagement;

public record OrderDraftDto(string Id, DateTimeOffset CreatedOn, DateTimeOffset UpdatedOn,
    OrderUserDto Supplier, OrderUserDto Customer, IEnumerable<OrderDraftLineDto> Lines);