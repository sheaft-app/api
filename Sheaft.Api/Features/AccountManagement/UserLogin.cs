using System.Text.Json.Serialization;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.AccountManagement;
using Sheaft.Domain;

namespace Sheaft.Api.AccountManagement;

[Route(Routes.TOKEN)]
public class UserLogin : Feature
{
    public UserLogin(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Post([FromBody] LoginRequest data, CancellationToken token)
    {
        var result = await Mediator.Execute(data.Adapt<LoginUserCommand>(), token);
        return HandleCommandResult<AuthenticationTokenDto, LoginResponse>(result);
    }
}

public record LoginRequest(string Username, string Password);

public record LoginResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; }
    
    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; }
    
    [JsonPropertyName("token_type")]
    public string TokenType { get; }
    
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; }
}