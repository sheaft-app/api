namespace Sheaft.Domain;

public record InvoiceId(string Value)
{
    public static InvoiceId New()
    {
        return new InvoiceId(Guid.NewGuid().ToString("N"));
    }
}