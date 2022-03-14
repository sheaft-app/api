namespace Sheaft.Domain.AccountManagement;

public class RefreshToken
{
    private RefreshToken()
    {
    }
    
    public RefreshToken(RefreshTokenId identifier, DateTimeOffset expiresOn)
    {
        Identifier = identifier;
        ExpiresOn = expiresOn;
    }

    public RefreshTokenId Identifier { get; private set; }
    public DateTimeOffset ExpiresOn { get; private set; }
    public bool Expired { get; private set; }

    public void Expire()
    {
        Expired = true;
    }
}