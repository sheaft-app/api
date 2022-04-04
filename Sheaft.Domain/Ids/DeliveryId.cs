namespace Sheaft.Domain;

public record DeliveryId(string Value)
{
    public static DeliveryId New()
    {
        return new DeliveryId(Guid.NewGuid().ToString("N"));
    }
}