namespace Sheaft.Domain;

public record OrderReference
{
    private OrderReference(){}
    
    public OrderReference(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidOperationException("Order code is required");
        
        Value = value;
    }

    public string Value { get; }
}