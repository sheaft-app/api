namespace Sheaft.Domain;

public record CatalogId(string Value)
{
    public static CatalogId New()
    {
        return new CatalogId(Nanoid.Nanoid.Generate(Constants.IDS_ALPHABET, Constants.IDS_LENGTH));
    }
}