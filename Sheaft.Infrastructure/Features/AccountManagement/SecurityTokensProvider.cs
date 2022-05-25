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

    public AccessToken GenerateAccessToken(Account account, Profile? profile)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, account.Identifier.Value),
            new Claim(ClaimTypes.Name, $"{account.Firstname.Value} {account.Lastname.Value}"),
            new Claim(ClaimTypes.Email, account.Email.Value),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
            new Claim(ClaimTypes.NameIdentifier, account.Username.Value),
            new Claim(ClaimTypes.GivenName, account.Firstname.Value),
            new Claim(ClaimTypes.Surname, account.Lastname.Value)
        };
        
        if (profile != null)
        {
            claims.Add(new Claim(CustomClaims.ProfileIdentifier, profile.Identifier));
            claims.Add(new Claim(CustomClaims.ProfileKind, profile.Kind.ToString("G")));
            claims.Add(new Claim(CustomClaims.ProfileName, profile.Name));
            claims.Add(new Claim(CustomClaims.ProfileStatus, "Registered"));
        }
        else
        {
            claims.Add(new Claim(CustomClaims.ProfileStatus, "Anonymous"));
        }

        var expires = DateTimeOffset.UtcNow.AddMinutes(_securitySettings.AccessTokenExpirationInMinutes);
        
        var accessToken = CreateToken(claims, expires);
        return new AccessToken(accessToken, "Bearer", _securitySettings.AccessTokenExpirationInMinutes);
    }

    public RefreshToken GenerateRefreshToken(AccountId accountIdentifier)
    {
        var tokenId = RefreshTokenId.New();
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, accountIdentifier.Value),
            new Claim(JwtRegisteredClaimNames.Jti, tokenId.Value)
        };
        
        var expires = DateTimeOffset.UtcNow.AddMinutes(_securitySettings.RefreshTokenExpirationInMinutes);

        var refreshToken = CreateToken(claims, expires);
        return new RefreshToken(new RefreshTokenInfo(tokenId, expires), refreshToken);
    }

    public (AccountId, RefreshTokenId) RetrieveTokenIdentifierData(string refreshToken)
    {
        var result = new JwtSecurityTokenHandler().ReadJwtToken(refreshToken);
        if (result == null)
            throw new InvalidOperationException("Cannot read provided refresh token");

        var usernameClaim = result.Claims.SingleOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
        var refreshTokenIdClaim = result.Claims.SingleOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti);
        
        if(usernameClaim == null || refreshTokenIdClaim == null)
            throw new InvalidOperationException("Provided refresh token malformed");

        return (new AccountId(usernameClaim.Value), new RefreshTokenId(refreshTokenIdClaim.Value));
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