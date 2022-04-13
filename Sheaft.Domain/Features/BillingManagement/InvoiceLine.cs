namespace Sheaft.Domain.InvoiceManagement;

public record InvoiceLine
{
    private InvoiceLine()
    {
    }

    public InvoiceLine(string name, Quantity quantity, UnitPrice unitPrice, VatRate vat, string? description = null)
    {
        Name = name;
        Description = description;
        PriceInfo = new LinePrice(unitPrice, vat, quantity);
    }

    public string Name { get; private set; }
    public string? Description { get; private set; }
    public LinePrice PriceInfo { get; private set; }
}