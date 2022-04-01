namespace Sheaft.Domain;

public record Quantity
{
    public Quantity(int value)
    {
        Value = value switch
        {
            < 0 => throw new InvalidOperationException("Quantity cannot be lower than 0"),
            > 10000 => throw new InvalidOperationException("Quantity cannot be greater than 1000"),
            _ => value
        };

        Value = value;
    }

    public int Value { get; }
}