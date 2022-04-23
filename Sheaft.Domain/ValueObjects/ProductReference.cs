namespace Sheaft.Domain;

public record ProductReference
{
    public const int MAXLENGTH = 20;
    private ProductReference(){}
    
    public ProductReference(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidOperationException("Product code is required");

        Value = value;
    }

    public string Value { get; }
}