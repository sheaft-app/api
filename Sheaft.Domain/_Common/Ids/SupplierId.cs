namespace Sheaft.Domain;

public record SupplierId(string Value)
{
    public static SupplierId New()
    {
        return new SupplierId(Guid.NewGuid().ToString("N"));
    }
}