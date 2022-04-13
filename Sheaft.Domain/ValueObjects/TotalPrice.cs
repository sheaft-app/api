namespace Sheaft.Domain;

public record LineWholeSalePrice
{
    public LineWholeSalePrice(UnitPrice unitPrice, Quantity quantity)
    {
        Value = unitPrice.Value * quantity.Value;
    }
    
    public LineWholeSalePrice(int value)
    {
        Value = value;
    }

    public int Value { get; }
}

public record TotalWholeSalePrice
{
    public TotalWholeSalePrice(int value)
    {
        Value = value;
    }

    public int Value { get; }
}