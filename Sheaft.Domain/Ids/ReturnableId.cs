namespace Sheaft.Domain;

public record ReturnableId(string Value)
{
    public static ReturnableId New()
    {
        return new ReturnableId(Nanoid.Nanoid.Generate(Constants.IDS_ALPHABET, Constants.IDS_LENGTH));
    }
}