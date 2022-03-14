namespace Sheaft.Domain;

public record OrderId(string Value)
{
    public static OrderId New()
    {
        return new OrderId(Guid.NewGuid().ToString("N"));
    }
}