namespace Sheaft.Domain.ProductManagement;

public record ProductName
{
    private ProductName(){}

    public ProductName(string value)
    {
        Value = value;
    }

    public string Value { get; }
}