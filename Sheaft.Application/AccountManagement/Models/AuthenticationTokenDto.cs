using System.Text.Json.Serialization;

namespace Sheaft.Application.AccountManagement;

public record AuthenticationTokenDto
{
    public AuthenticationTokenDto(string accessToken, string refreshToken, string tokenType, int expiresIn)
    {
        AccessToken = accessToken;
        TokenType = tokenType;
        ExpiresIn = expiresIn;
        RefreshToken = refreshToken;
    }
    
    [JsonPropertyName("access_token")]
    public string AccessToken { get; }
    
    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; }
    
    [JsonPropertyName("token_type")]
    public string TokenType { get; }
    
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; }
}