namespace Sheaft.Domain;

public record TotalPrice
{
    public TotalPrice(int value) : this(value, Currency.Euro)
    {
    }
    
    public TotalPrice(int value, Currency currency)
    {
        Value = value;
        Currency = currency;
    }

    public int Value { get; }
    public Currency Currency { get; }
}