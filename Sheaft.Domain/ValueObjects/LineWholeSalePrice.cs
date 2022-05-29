namespace Sheaft.Domain;

public record LineWholeSalePrice
{
    public LineWholeSalePrice(UnitPrice unitPrice, Quantity quantity)
    {
        Value = Math.Round(unitPrice.Value * quantity.Value, 2, MidpointRounding.ToEven);
    }
    
    public LineWholeSalePrice(decimal value)
    {
        Value = Math.Round(value, 2, MidpointRounding.ToEven);
    }

    public decimal Value { get; }
}