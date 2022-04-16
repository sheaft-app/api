namespace Sheaft.Domain.InvoiceManagement;

public record InvoiceDelivery(DeliveryReference Reference, DateTimeOffset DeliveredOn);