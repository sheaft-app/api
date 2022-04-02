namespace Sheaft.Domain;

public record OrderDeliveryDate
{
    private OrderDeliveryDate(){}

    public OrderDeliveryDate(DateTimeOffset value)
        :this(value, DateTimeOffset.UtcNow)
    {
    }

    public OrderDeliveryDate(DateTimeOffset value, DateTimeOffset currentDateTime)
    {
        if (value.Date < currentDateTime.Date)
            throw new InvalidOperationException("Order delivery date must be in future");
        
        Value = DateOnly.FromDateTime(value.UtcDateTime);
    }

    public DateOnly Value { get; }
}