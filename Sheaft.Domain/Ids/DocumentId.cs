namespace Sheaft.Domain;

public record DocumentId(string Value)
{
    public static DocumentId New()
    {
        return new DocumentId(Nanoid.Nanoid.Generate(Constants.IDS_ALPHABET, Constants.IDS_LENGTH));
    }
}