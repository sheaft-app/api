namespace Sheaft.Domain;

public record CustomerId(string Value)
{
    public static CustomerId New()
    {
        return new CustomerId(Guid.NewGuid().ToString("N"));
    }
}