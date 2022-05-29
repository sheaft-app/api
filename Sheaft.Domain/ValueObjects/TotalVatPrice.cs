namespace Sheaft.Domain;

public record TotalVatPrice
{
    public TotalVatPrice(decimal value)
    {
        Value = value;
    }

    public decimal Value { get; }
}