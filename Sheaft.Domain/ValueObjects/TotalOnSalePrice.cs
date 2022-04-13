namespace Sheaft.Domain;

public record LineOnSalePrice
{
    public LineOnSalePrice(UnitPrice unitPrice, Quantity quantity, VatRate vat)
    {
        Value = unitPrice.Value * (1 + vat.Value / 10000) * quantity.Value;
    }
    
    public LineOnSalePrice(int value)
    {
        Value = value;
    }

    public int Value { get; }
}

public record TotalOnSalePrice
{
    public TotalOnSalePrice(int value)
    {
        Value = value;
    }

    public int Value { get; }
}