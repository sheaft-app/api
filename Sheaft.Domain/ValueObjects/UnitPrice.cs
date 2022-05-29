namespace Sheaft.Domain;

public record UnitPrice
{
    public UnitPrice(decimal value)
    {
        if (value < 0)
            throw new InvalidOperationException("Price cannot be lower than 0");

        Value = Math.Round(value, 2, MidpointRounding.ToEven);
    }

    public decimal Value { get; }
}