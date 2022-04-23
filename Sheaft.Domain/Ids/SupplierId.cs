namespace Sheaft.Domain;

public record SupplierId(string Value)
{
    public static SupplierId New()
    {
        return new SupplierId(Nanoid.Nanoid.Generate(Constants.IDS_ALPHABET, Constants.IDS_LENGTH));
    }
}