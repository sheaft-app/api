namespace Sheaft.Domain;

public record DocumentId(string Value)
{
    public static DocumentId New()
    {
        return new DocumentId(Guid.NewGuid().ToString("N"));
    }
}