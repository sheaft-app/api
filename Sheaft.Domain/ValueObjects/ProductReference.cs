namespace Sheaft.Domain;

public record ProductReference
{
    private ProductReference(){}
    
    public ProductReference(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidOperationException("Product code is required");

        Value = value;
    }

    public string Value { get; }
}