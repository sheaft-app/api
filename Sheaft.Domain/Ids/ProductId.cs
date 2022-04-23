namespace Sheaft.Domain;

public record ProductId(string Value)
{
    public static ProductId New()
    {
        return new ProductId(Nanoid.Nanoid.Generate(Constants.IDS_ALPHABET, Constants.IDS_LENGTH));
    }
}