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

        if (value != 0 && value != 550 && value != 700 && value != 1000 && value != 2000)
            throw new InvalidOperationException("VAT Rate is invalid");
    }

    public int Value { get; }
}