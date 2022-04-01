namespace Sheaft.Domain;

public record ProductCode
{
    private ProductCode(){}
    
    public ProductCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidOperationException("Product code is required");

        Value = value;
    }

    public string Value { get; }
}