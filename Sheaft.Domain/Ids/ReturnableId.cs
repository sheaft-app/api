namespace Sheaft.Domain;

public record ReturnableId(string Value)
{
    public static ReturnableId New()
    {
        return new ReturnableId(Guid.NewGuid().ToString("N"));
    }
}