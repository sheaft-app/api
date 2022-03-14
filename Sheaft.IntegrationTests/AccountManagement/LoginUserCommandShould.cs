using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Sheaft.Application.AccountManagement;
using Sheaft.Infrastructure;
using Sheaft.Infrastructure.AccountManagement;

namespace Sheaft.IntegrationTests.AccountManagement;

#pragma warning disable CS8767
#pragma warning disable CS8618

public class LoginUserCommandShould
{
    [Test]
    public async Task Log_User_In()
    {
        var (email, pwd, handler) = InitHandler();

        var result = await handler.Handle(new LoginUserCommand(email, pwd),
            CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
    }

    [Test]
    public async Task Fail_If_Password_Is_Invalid()
    {
        var (email, pwd, handler) = InitHandler();

        var result = await handler.Handle(new LoginUserCommand(email, "test"),
            CancellationToken.None);
        
        Assert.IsTrue(result.IsFailure);
    }

    private (string, string, LoginUserHandler) InitHandler()
    {
        var username = "username";
        var hasher = new PasswordHasher("my_salt_value");
        var handler = new LoginUserHandler(
            null,
            hasher,
            new SecurityTokensProvider(new JwtSettings
            {
                Issuer = "http",
                Salt = "my_salt_value",
                Secret = "my_super_secret_value"
            }, new SecuritySettings
            {
                AccessTokenExpirationInMinutes = 5,
                RefreshTokenExpirationInMinutes = 5,
                ResetPasswordTokenValidityInHours = 5,
            }));
        
        return (username, "password", handler);
    }
}
#pragma warning restore CS8767
#pragma warning restore CS8618