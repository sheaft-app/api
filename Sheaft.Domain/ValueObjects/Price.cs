namespace Sheaft.Domain;

public record Price
{
    public Price(decimal value)
    {
        Value = Math.Round(value, 2, MidpointRounding.ToEven);
    }

    public decimal Value { get; }
}