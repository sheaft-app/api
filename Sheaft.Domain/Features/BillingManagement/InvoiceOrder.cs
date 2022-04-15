namespace Sheaft.Domain.InvoiceManagement;

public record InvoiceOrder(OrderReference Reference, DateTimeOffset PublishedOn);