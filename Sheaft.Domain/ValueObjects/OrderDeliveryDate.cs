namespace Sheaft.Domain;

public record OrderDeliveryDate
{
    private OrderDeliveryDate(){}
    
    public OrderDeliveryDate(DateTimeOffset value)
    {
        if (value.UtcDateTime < DateTime.UtcNow)
            throw new InvalidOperationException("Order delivery date must be in future");
        
        Value = DateOnly.FromDateTime(value.UtcDateTime);
    }

    public DateOnly Value { get; }
}