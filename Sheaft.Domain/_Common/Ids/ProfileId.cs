namespace Sheaft.Domain;

public record ProfileId(string Value)
{
    public static ProfileId New()
    {
        return new ProfileId(Guid.NewGuid().ToString("N"));
    }
}