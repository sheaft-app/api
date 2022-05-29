namespace Sheaft.Domain.BillingManagement;

public record InvoiceDelivery(DeliveryReference Reference, DateTimeOffset DeliveredOn);