using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Sheaft.Domain;
using Sheaft.Domain.AccountManagement;

namespace Sheaft.Infrastructure.AccountManagement;

public class SecurityTokensProvider : ISecurityTokensProvider
{
    private readonly JwtSettings _jwtSettings;
    private readonly SecuritySettings _securitySettings;

    public SecurityTokensProvider(IOptionsSnapshot<JwtSettings> jwtSettings, IOptionsSnapshot<SecuritySettings> securitySettings)
    {
        _jwtSettings = jwtSettings.Value;
        _securitySettings = securitySettings.Value;
    }
    
    public SecurityTokensProvider(JwtSettings jwtSettings, SecuritySettings securitySettings)
    {
        _jwtSettings = jwtSettings;
        _securitySettings = securitySettings;
    }

    public AccessToken GenerateAccessToken(Account account)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, account.Profile.Identifier.Value),
            new Claim(ClaimTypes.Name, account.Username.Value),
            new Claim(ClaimTypes.Email, account.Email.Value),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
            new Claim(ClaimTypes.NameIdentifier, account.Profile.Identifier.Value),
            //TODO new Claim("GroupIdentifier", account.Group.Value),
        };

        var expires = DateTimeOffset.UtcNow.AddMinutes(_securitySettings.AccessTokenExpirationInMinutes);
        
        var accessToken = CreateToken(claims, expires);
        return new AccessToken(accessToken, "Bearer", _securitySettings.AccessTokenExpirationInMinutes);
    }

    public RefreshToken GenerateRefreshToken(Username username)
    {
        var tokenId = RefreshTokenId.New();
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username.Value),
            new Claim(JwtRegisteredClaimNames.Jti, tokenId.Value)
        };
        
        var expires = DateTimeOffset.UtcNow.AddMinutes(_securitySettings.RefreshTokenExpirationInMinutes);

        var refreshToken = CreateToken(claims, expires);
        return new RefreshToken(new RefreshTokenInfo(tokenId, expires), refreshToken);
    }

    public (Username, RefreshTokenId) RetrieveTokenIdentifierData(string refreshToken)
    {
        var result = new JwtSecurityTokenHandler().ReadJwtToken(refreshToken);
        if (result == null)
            throw new InvalidOperationException("Cannot read provided refresh token");

        var usernameClaim = result.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Name);
        var refreshTokenIdClaim = result.Claims.SingleOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti);
        
        if(usernameClaim == null || refreshTokenIdClaim == null)
            throw new InvalidOperationException("Provided refresh token malformed");

        return (new Username(usernameClaim.Value), new RefreshTokenId(refreshTokenIdClaim.Value));
    }

    private string CreateToken(List<Claim> claims, DateTimeOffset expires)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var authToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Issuer,
            claims,
            notBefore: DateTime.UtcNow,
            expires: expires.DateTime,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(authToken);
    }
}