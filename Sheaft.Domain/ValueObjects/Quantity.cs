namespace Sheaft.Domain;

public record Quantity
{
    public Quantity(int value)
    {
        Value = value switch
        {
            > 1000 => throw new InvalidOperationException("Quantity cannot be greater than 1000"),
            < -1000 => throw new InvalidOperationException("Quantity cannot be lower than 1000"),
            _ => value
        };

        Value = value;
    }

    public int Value { get; private set; }

    public void Update(Quantity quantity)
    {
        Value = quantity.Value;
    }
}