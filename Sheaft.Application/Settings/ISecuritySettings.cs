namespace Sheaft.Application;

public interface ISecuritySettings
{
    public int ResetPasswordTokenValidityInHours { get; set; }
    public int AccessTokenExpirationInMinutes { get; set; }
    public int RefreshTokenExpirationInMinutes { get; set; }
}