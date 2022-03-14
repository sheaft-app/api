namespace Sheaft.Domain;

public record RefreshTokenId
{
    private RefreshTokenId(){}
    
    public RefreshTokenId(string value)
    {
        Value = value;
    }

    public string Value { get; }
    
    public static RefreshTokenId New()
    {
        return new RefreshTokenId(Guid.NewGuid().ToString("N"));
    }
}