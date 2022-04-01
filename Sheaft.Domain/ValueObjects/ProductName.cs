namespace Sheaft.Domain;

public record ProductName
{
    private ProductName(){}

    public ProductName(string value)
    {
        Value = value;
    }

    public string Value { get; }
}