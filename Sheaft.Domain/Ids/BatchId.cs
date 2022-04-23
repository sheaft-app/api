namespace Sheaft.Domain;

public record BatchId(string Value)
{
    public static BatchId New()
    {
        return new BatchId(Nanoid.Nanoid.Generate(Constants.IDS_ALPHABET, Constants.IDS_LENGTH));
    }
}