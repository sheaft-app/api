namespace Sheaft.Domain;

public record RefreshTokenId(string Value)
{
    public static RefreshTokenId New()
    {
        return new RefreshTokenId(Guid.NewGuid().ToString("N"));
    }
}