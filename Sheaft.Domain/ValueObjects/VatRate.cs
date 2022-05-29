namespace Sheaft.Domain;

public record VatRate
{
    public VatRate(decimal value)
    {
        Value = value switch
        {
            < 0 => throw new InvalidOperationException("VAT Rate cannot be lower than 0%"),
            > 100 => throw new InvalidOperationException("VAT Rate cannot be greater than 100%"),
            _ => value
        };

        if (value != 0 && value != 5.5m && value != 10 && value != 20)
            throw new InvalidOperationException("VAT Rate is invalid");
    }

    public decimal Value { get; }
}