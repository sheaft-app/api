namespace Sheaft.Domain;

public record ProductUnitPrice : UnitPrice
{
    public ProductUnitPrice(int value) : this(value, Currency.Euro)
    {
    }

    public ProductUnitPrice(int value, Currency currency) : base(value, currency)
    {
        if(value <= 0)
            throw new InvalidOperationException("Product price must be greater than 0");
    }
}