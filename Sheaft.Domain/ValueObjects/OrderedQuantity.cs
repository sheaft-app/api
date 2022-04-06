namespace Sheaft.Domain;

public record OrderedQuantity : Quantity
{
    public OrderedQuantity(int value)
        : base(value)
    {
        Value = value switch
        {
            < 0 => throw new InvalidOperationException("Quantity cannot be lower than 0"),
            _ => value
        };

        Value = value;
    }

    public int Value { get; }
}