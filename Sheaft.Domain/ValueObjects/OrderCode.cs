namespace Sheaft.Domain;

public record OrderCode
{
    private OrderCode(){}
    
    public OrderCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidOperationException("Order code is required");
        
        Value = value;
    }

    public string Value { get; }
}