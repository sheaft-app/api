namespace Sheaft.Domain;

public record ProductPrice : Price
{
    public ProductPrice(int value) : this(value, Currency.Euro)
    {
    }

    public ProductPrice(int value, Currency currency) : base(value, currency)
    {
        if(value <= 0)
            throw new InvalidOperationException("Product price must be greater than 0");
    }
}