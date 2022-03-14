using Sheaft.Application;

namespace Sheaft.Infrastructure;

#pragma warning disable CS8767
#pragma warning disable CS8618
public class SecuritySettings : ISecuritySettings
{
    public const string SECTION = "Security";
    
    public int ResetPasswordTokenValidityInHours { get; set; }
    public int AccessTokenExpirationInMinutes { get; set; }
    public int RefreshTokenExpirationInMinutes { get; set; }
}
#pragma warning restore CS8767
#pragma warning restore CS8618