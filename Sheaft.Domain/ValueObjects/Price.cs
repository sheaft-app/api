namespace Sheaft.Domain;

public record Price
{
    public Price(int value)
    {
        Value = value;
    }

    public int Value { get; }
}