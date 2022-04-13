namespace Sheaft.Domain;

public record LineVatPrice
{
    public LineVatPrice(UnitPrice unitPrice, Quantity quantity, VatRate vat)
    {
        Value = unitPrice.Value * (vat.Value / 10000) * quantity.Value;
    }

    public LineVatPrice(int value)
    {
        Value = value;
    }

    public int Value { get; }
}

public record TotalVatPrice
{
    public TotalVatPrice(int value)
    {
        Value = value;
    }

    public int Value { get; }
}