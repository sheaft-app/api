namespace Sheaft.Domain;

public record CustomerId(string Value)
{
    public static CustomerId New()
    {
        return new CustomerId(Nanoid.Nanoid.Generate(Constants.IDS_ALPHABET, Constants.IDS_LENGTH));
    }
}