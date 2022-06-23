using System.Security.Claims;

namespace Sheaft.Infrastructure;

public class CustomClaims
{
    private const string _namespace = "http://schemas.sheaft.com/ws/identity/claims";
    public const string AccountIdentifier = ClaimTypes.NameIdentifier;
    public const string ProfileIdentifier = $"{_namespace}/profile/identifier";
    public const string ProfileKind = $"{_namespace}/profile/kind";
    public const string ProfileName = $"{_namespace}/profile/name";
    public const string ProfileStatus = $"{_namespace}/profile/status";
}