namespace Sheaft.Domain;

public record TotalWholeSalePrice
{
    public TotalWholeSalePrice(decimal value)
    {
        Value = value;
    }

    public decimal Value { get; }
}