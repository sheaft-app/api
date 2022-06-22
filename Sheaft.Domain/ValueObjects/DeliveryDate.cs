namespace Sheaft.Domain;

public record DeliveryDate
{
    private DeliveryDate(){}

    public DeliveryDate(DateTimeOffset value)
        :this(value, DateTimeOffset.UtcNow)
    {
    }

    public DeliveryDate(DateTimeOffset value, DateTimeOffset currentDateTime)
    {
        if (value.Date < currentDateTime.Date)
            throw new InvalidOperationException("Delivery date must be in future");
        
        Value = value;
    }

    public DateTimeOffset Value { get; }
}