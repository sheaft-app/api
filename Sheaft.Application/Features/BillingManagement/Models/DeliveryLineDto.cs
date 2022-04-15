namespace Sheaft.Application.InvoiceManagement;

public record InvoiceLineDto(string Identifier, string Name, int Quantity, int Vat, int UnitPrice);