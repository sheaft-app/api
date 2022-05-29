namespace Sheaft.Domain;

public record ProductUnitPrice : UnitPrice
{
    public ProductUnitPrice(UnitPrice value) : this(value.Value)
    {
    }

    public ProductUnitPrice(decimal value) : base(value)
    {
        if(value <= 0)
            throw new InvalidOperationException("Product price must be greater than 0");
    }
}