using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Sheaft.Application.AccountManagement;
using Sheaft.Domain;
using Sheaft.Domain.AccountManagement;
using Sheaft.Domain.ProfileManagement;
using Sheaft.Infrastructure;
using Sheaft.Infrastructure.AccountManagement;

namespace Sheaft.IntegrationTests.AccountManagement;

#pragma warning disable CS8767
#pragma warning disable CS8618

public class RefreshAccessTokenCommandShould
{
    [Test]
    public async Task Generate_New_AccessToken_And_RefreshToken()
    {
        var (handler, authenticationToken, _) = InitHandler();
        
        var result = await handler.Handle(new RefreshAccessTokenCommand(authenticationToken.RefreshToken),
            CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreNotEqual(authenticationToken.AccessToken, result.Value.AccessToken);
        Assert.AreNotEqual(authenticationToken.RefreshToken, result.Value.RefreshToken);
    }
    
    [Test]
    public async Task Should_Fail_If_UserId_In_Token_NotFound()
    {
        var (handler, _, securityTokensProvider) = InitHandler();

        var (data, token) = securityTokensProvider.GenerateRefreshToken(new Username("test"));
        var result = await handler.Handle(new RefreshAccessTokenCommand(token),
            CancellationToken.None);

        Assert.IsTrue(result.IsFailure);
    }
    
    [Test]
    public async Task Should_Cancel_All_RefreshToken_If_AlreadyExpired_Token_Provided()
    {
        var (handler, authenticationToken, _) = InitHandler();

        var result = await handler.Handle(new RefreshAccessTokenCommand(authenticationToken.RefreshToken),
            CancellationToken.None);
        Assert.IsTrue(result.IsSuccess);

        var refreshResult = await handler.Handle(new RefreshAccessTokenCommand(authenticationToken.RefreshToken),
            CancellationToken.None);
        
        Assert.IsTrue(refreshResult.IsFailure);
    }

    private (RefreshAccessTokenHandler, AuthenticationToken, SecurityTokensProvider) InitHandler()
    {
        var securityTokensProvider = new SecurityTokensProvider(new JwtSettings
        {
            Issuer = "http://localhost",
            Secret = "this_is_a_super_long_secret"
        }, new SecuritySettings
        {
            AccessTokenExpirationInMinutes = 5,
            RefreshTokenExpirationInMinutes = 15
        });

        Profile profile = null;
        var passwordHasher = new PasswordHasher("this_salt");
        var account = Account.Create(new Username("test"), new EmailAddress("test@test.com"),
            HashedPassword.Create("P@ssword", passwordHasher), profile);

        var loginResult = account.Login("P@ssword", passwordHasher, securityTokensProvider);
        
        var handler = new RefreshAccessTokenHandler(null, securityTokensProvider);
            
        return (handler, loginResult.Value, securityTokensProvider);
    }
}
#pragma warning restore CS8767
#pragma warning restore CS8618