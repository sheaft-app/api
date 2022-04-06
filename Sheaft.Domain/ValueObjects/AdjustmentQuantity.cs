namespace Sheaft.Domain;

public record AdjustmentQuantity
{
    public AdjustmentQuantity(int value)
    {
        Value = value switch
        {
            >= 0 => throw new InvalidOperationException("AdjustmentQuantity Quantity cannot be greater than 0"),
            < -1000 => throw new InvalidOperationException("AdjustmentQuantity Quantity cannot be lower than 1000"),
            _ => value
        };

        Value = value;
    }

    public int Value { get; }
}