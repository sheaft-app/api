using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Sheaft.Application.AccountManagement;
using Sheaft.Domain;
using Sheaft.Domain.AccountManagement;
using Sheaft.Domain.SupplierManagement;
using Sheaft.Infrastructure;
using Sheaft.Infrastructure.AccountManagement;
using Sheaft.UnitTests.Fakes;
using Sheaft.UnitTests.Helpers;

namespace Sheaft.UnitTests.AccountManagement;

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
    public async Task Fail_If_UserId_In_Token_NotFound()
    {
        var (handler, _, securityTokensProvider) = InitHandler();

        var (data, token) = securityTokensProvider.GenerateRefreshToken(new AccountId("test"));
        var result = await handler.Handle(new RefreshAccessTokenCommand(token),
            CancellationToken.None);

        Assert.IsTrue(result.IsFailure);
    }
    
    [Test]
    public async Task Cancel_All_RefreshToken_If_Provided_Token_Is_Expired()
    {
        var (handler, authenticationToken, _) = InitHandler();

        var result = await handler.Handle(new RefreshAccessTokenCommand(authenticationToken.RefreshToken),
            CancellationToken.None);
        Assert.IsTrue(result.IsSuccess);

        var refreshResult = await handler.Handle(new RefreshAccessTokenCommand(authenticationToken.RefreshToken),
            CancellationToken.None);
        
        Assert.IsTrue(refreshResult.IsFailure);
    }

    private (RefreshAccessTokenHandler, AuthenticationResult, SecurityTokensProvider) InitHandler()
    {
        var securityTokensProvider = new SecurityTokensProvider(new JwtSettings
        {
            Issuer = "http://localhost",
            Salt = "my_salt_value",
            Secret = "this_is_a_super_long_secret"
        }, new SecuritySettings
        {
            AccessTokenExpirationInMinutes = 5,
            RefreshTokenExpirationInMinutes = 15
        });

        var context = new FakeDbContextFactory().CreateContext();
        
        var username = "test@est.com";
        var password = "password";
        
        var hasher = new PasswordHasher("my_salt_value");
        var account = DataHelpers.GetDefaultAccount(hasher, username, password);
        
        context.Accounts.Add(account);
        context.Suppliers.Add(DataHelpers.GetDefaultSupplier(account.Identifier));
        context.SaveChanges();
        
        var loginResult = account.Login(password, hasher, securityTokensProvider, new Profile("test", "name", ProfileKind.Supplier));

        var handler = new RefreshAccessTokenHandler(new UnitOfWork(new FakeMediator(), context, new FakeLogger<UnitOfWork>()), securityTokensProvider, new RetrieveProfile(context));
            
        return (handler, loginResult.Value, securityTokensProvider);
    }
}
#pragma warning restore CS8767
#pragma warning restore CS8618