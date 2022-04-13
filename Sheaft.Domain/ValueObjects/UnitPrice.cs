namespace Sheaft.Domain;

public record UnitPrice
{
    public UnitPrice(int value)
    {
        if (value < 0)
            throw new InvalidOperationException("Price cannot be lower than 0");

        Value = value;
    }

    public int Value { get; }
}