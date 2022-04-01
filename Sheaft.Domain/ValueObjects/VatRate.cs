namespace Sheaft.Domain;

public record VatRate
{
    public VatRate(int value)
    {
        Value = value switch
        {
            < 0 => throw new InvalidOperationException("VAT Rate cannot be lower than 0%"),
            > 10000 => throw new InvalidOperationException("VAT Rate cannot be greater than 100%"),
            _ => value
        };
    }

    public int Value { get; }
}