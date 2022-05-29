namespace Sheaft.Domain;

public record TotalOnSalePrice
{
    public TotalOnSalePrice(decimal value)
    {
        Value = value;
    }

    public decimal Value { get; }
}