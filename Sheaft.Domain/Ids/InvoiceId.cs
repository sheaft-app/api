namespace Sheaft.Domain;

public record InvoiceId(string Value)
{
    public static InvoiceId New()
    {
        return new InvoiceId(Nanoid.Nanoid.Generate(Constants.IDS_ALPHABET, Constants.IDS_LENGTH));
    }
}