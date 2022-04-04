namespace Sheaft.Domain;

public record DeliveryCode
{
    private DeliveryCode(){}
    
    public DeliveryCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidOperationException("Delivery code is required");
        
        Value = value;
    }

    public string Value { get; }
}