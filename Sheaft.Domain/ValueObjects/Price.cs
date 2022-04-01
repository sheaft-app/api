namespace Sheaft.Domain;

public record Price
{
    public Price(int value) : this(value, Currency.Euro)
    {
    }
    
    public Price(int value, Currency currency)
    {
        if (value < 0)
            throw new InvalidOperationException("Price cannot be lower than 0");

        Value = value;
        Currency = currency;
    }

    public int Value { get; }
    public Currency Currency { get; }
}