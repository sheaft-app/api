namespace Sheaft.Domain;

public record RetailerId(string Value)
{
    public static RetailerId New()
    {
        return new RetailerId(Guid.NewGuid().ToString("N"));
    }
}