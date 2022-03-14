using Sheaft.Application;

namespace Sheaft.Infrastructure;

#pragma warning disable CS8767
#pragma warning disable CS8618

public class JwtSettings : IJwtSettings
{
    public const string SECTION = "Jwt";
    public string Issuer { get; set; }
    public string Secret { get; set; }
    public string Salt { get; set; }
}
#pragma warning restore CS8767
#pragma warning restore CS8618