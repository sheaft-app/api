using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Sheaft.Web.Api.AccountManagement;

[SuppressMessage("ReSharper", "UnassignedGetOnlyAutoProperty")]
public record TokenResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }
    
    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; }
    
    [JsonPropertyName("token_type")]
    public string TokenType { get; set; }
    
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }
}