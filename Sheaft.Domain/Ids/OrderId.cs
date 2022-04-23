namespace Sheaft.Domain;

public record OrderId(string Value)
{
    public static OrderId New()
    {
        return new OrderId(Nanoid.Nanoid.Generate(Constants.IDS_ALPHABET, Constants.IDS_LENGTH));
    }
}