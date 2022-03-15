using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Sheaft.Application.AccountManagement;
using Sheaft.Domain;
using Sheaft.Infrastructure;
using Sheaft.Infrastructure.AccountManagement;
using Sheaft.IntegrationTests.Fakes;
using Sheaft.IntegrationTests.Helpers;

namespace Sheaft.IntegrationTests.AccountManagement;

#pragma warning disable CS8767
#pragma warning disable CS8618

public class GetProfileQueryShould
{
    // [Test]
    // public async Task Return_User_Profile()
    // {
    //     var (profileId, handler) = InitHandler();
    //
    //     var result = await handler.Handle(new GetProfileQuery(profileId.Value),
    //         CancellationToken.None);
    //
    //     Assert.IsTrue(result.IsSuccess);
    // }
    //
    // [Test]
    // public async Task Fail_If_User_Not_Found()
    // {
    //     var (_, handler) = InitHandler();
    //
    //     var result = await handler.Handle(new GetProfileQuery(ProfileId.New().Value),
    //         CancellationToken.None);
    //     
    //     Assert.IsTrue(result.IsFailure);
    // }

    private (ProfileId, GetProfileHandler) InitHandler()
    {
        var username = "test@est.com";
        var password = "password";
        
        var hasher = new PasswordHasher("my_salt_value");
        var account = AccountTests.GetDefaultAccount(hasher, username, password);

        var connectionFactory = new SqliteDbConnectionFactory();

        var connection = connectionFactory.CreateConnection(DatabaseConnectionName.AppDatabase);
        
        var factory = new FakeDbContextFactory();
        var context = factory.CreateContext(connection);
        
        context.Accounts.Add(account);
        context.SaveChanges();
        
        var handler = new GetProfileHandler(new ProfileQueries(connectionFactory));
        
        return (account.Profile.Identifier, handler);
    }
}
#pragma warning restore CS8767
#pragma warning restore CS8618