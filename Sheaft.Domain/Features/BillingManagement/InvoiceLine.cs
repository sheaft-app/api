namespace Sheaft.Domain.InvoiceManagement;

public record InvoiceLine
{
    private InvoiceLine()
    {
    }

    protected InvoiceLine(bool editable, string identifier, string name, Quantity quantity, UnitPrice unitPrice,
        VatRate vat)
    {
        Identifier = identifier;
        Name = name;
        Quantity = quantity;
        Vat = vat;
        PriceInfo = new LinePrice(unitPrice, vat, quantity);
        IsEditable = editable;
    }

    public static InvoiceLine CreateLine(string name, Quantity quantity, UnitPrice unitPrice, VatRate vat)
    {
        return new InvoiceLine(true, Guid.NewGuid().ToString("N"), name, quantity, unitPrice, vat);
    }

    public static LockedInvoiceLine CreateLockedLine(string name, Quantity quantity, UnitPrice unitPrice, VatRate vat, string? identifier = null)
    {
        return new LockedInvoiceLine(identifier ?? Guid.NewGuid().ToString("N"), name, quantity, unitPrice, vat);
    }

    public string Identifier { get; private set; }
    public string Name { get; private set; }
    public LinePrice PriceInfo { get; private set; }
    public Quantity Quantity { get; }
    public VatRate Vat { get; }
    public bool IsEditable { get; }
}

public record LockedInvoiceLine(string Identifier, string Name, Quantity Quantity, UnitPrice UnitPrice, VatRate Vat):InvoiceLine(false, Identifier, Name, Quantity, UnitPrice, Vat);