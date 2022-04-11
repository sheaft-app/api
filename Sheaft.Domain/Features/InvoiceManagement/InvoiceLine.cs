namespace Sheaft.Domain.InvoiceManagement;

public record InvoiceLine
{
    private InvoiceLine()
    {
    }

    internal InvoiceLine(string identifier, string reference, string name, Quantity quantity, UnitPrice unitPrice, VatRate vat)
    {
        Identifier = identifier;
        Reference = reference;
        Name = name;
        Quantity = quantity;
        UnitPrice = unitPrice;
        Vat = vat;
        TotalPrice = GetTotalPrice(Quantity, UnitPrice);
    }

    public string Identifier { get; private set; }
    public string Reference { get; private set; }
    public string Name { get; private set; }
    public Quantity Quantity { get; private set; }
    public UnitPrice UnitPrice { get; private set; }
    public VatRate Vat { get; private set; }
    public TotalPrice TotalPrice { get; private set; }

    private static TotalPrice GetTotalPrice(Quantity quantity, UnitPrice? unitPrice)
    {
        return new TotalPrice((unitPrice?.Value ?? 0) * quantity.Value, unitPrice?.Currency ?? Currency.Euro);
    }
}