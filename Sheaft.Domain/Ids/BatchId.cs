namespace Sheaft.Domain;

public record BatchId(string Value)
{
    public static BatchId New()
    {
        return new BatchId(Guid.NewGuid().ToString("N"));
    }
}