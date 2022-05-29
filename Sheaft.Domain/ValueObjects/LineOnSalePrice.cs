namespace Sheaft.Domain;

public record LineOnSalePrice
{
    public LineOnSalePrice(UnitPrice unitPrice, Quantity quantity, VatRate vat)
    {
        Value = Math.Round(unitPrice.Value * (1 + vat.Value / 100) * quantity.Value, 2, MidpointRounding.ToEven);
    }
    
    public LineOnSalePrice(decimal value)
    {
        Value = Math.Round(value, 2, MidpointRounding.ToEven);
    }

    public decimal Value { get; }
}