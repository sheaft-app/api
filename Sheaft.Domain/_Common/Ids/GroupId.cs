namespace Sheaft.Domain;

public record GroupId(string Value)
{
    public static GroupId New()
    {
        return new GroupId(Guid.NewGuid().ToString("N"));
    }
}