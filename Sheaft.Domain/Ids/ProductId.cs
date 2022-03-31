namespace Sheaft.Domain;

public record ProductId(string Value)
{
    public static ProductId New()
    {
        return new ProductId(Guid.NewGuid().ToString("N"));
    }
}