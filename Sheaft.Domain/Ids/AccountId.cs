namespace Sheaft.Domain;

public record AccountId(string Value)
{
    public static AccountId New()
    {
        return new AccountId(Nanoid.Nanoid.Generate(Constants.IDS_ALPHABET, Constants.IDS_LENGTH));
    }
}