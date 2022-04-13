namespace Sheaft.Application.InvoiceManagement;

public record InvoiceLineDto(string Name, int Quantity, int Vat, int UnitPrice);