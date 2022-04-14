namespace Sheaft.Domain.InvoiceManagement;

public record InvoicePayment(string Reference, PaymentKind Kind, DateTimeOffset PaymentDate);