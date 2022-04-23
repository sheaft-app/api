namespace Sheaft.Domain.BillingManagement;

public record InvoicePayment(PaymentReference Reference, PaymentKind Kind, DateTimeOffset PaymentDate);