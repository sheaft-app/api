namespace Sheaft.Domain.BillingManagement;

public record InvoicePayment(string Reference, PaymentKind Kind, DateTimeOffset PaymentDate);