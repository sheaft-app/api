namespace Sheaft.Domain;

public class RefreshTokenInfo
{
    private RefreshTokenInfo()
    {
    }
    
    public RefreshTokenInfo(RefreshTokenId identifier, DateTimeOffset expiresOn)
    {
        Identifier = identifier;
        ExpiresOn = expiresOn;
        CreatedOn = DateTimeOffset.UtcNow;
        UpdatedOn = DateTimeOffset.UtcNow;
    }

    public RefreshTokenId Identifier { get; private set; }
    public DateTimeOffset CreatedOn { get; private set; }
    public DateTimeOffset UpdatedOn { get; private set; }
    public DateTimeOffset ExpiresOn { get; private set; }
    public bool Expired { get; private set; }

    public void Expire()
    {
        Expired = true;
        UpdatedOn = DateTimeOffset.UtcNow;
    }
}