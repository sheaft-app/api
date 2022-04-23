namespace Sheaft.Domain;

public record DeliveryReference
{
    public const int MAXLENGTH = 20;
    private DeliveryReference(){}
    
    public DeliveryReference(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidOperationException("Delivery code is required");
        
        Value = value;
    }

    public string Value { get; }
}