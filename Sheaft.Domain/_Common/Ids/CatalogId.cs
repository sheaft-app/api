namespace Sheaft.Domain;

public record CatalogId(string Value)
{
    public static CatalogId New()
    {
        return new CatalogId(Guid.NewGuid().ToString("N"));
    }
}