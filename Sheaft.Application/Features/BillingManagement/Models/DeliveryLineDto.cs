namespace Sheaft.Application.BillingManagement.Models;

public record InvoiceLineDto(string Identifier, string Name, int Quantity, int Vat, int UnitPrice);