using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Sheaft.Application.Models;
using Sheaft.Application.RetailerManagement;
using Sheaft.Domain;
using Sheaft.Domain.RetailerManagement;
using Sheaft.Infrastructure.Persistence;
using Sheaft.Infrastructure.RetailerManagement;
using Sheaft.IntegrationTests.Helpers;

namespace Sheaft.IntegrationTests.RetailerManagement;

#pragma warning disable CS8767
#pragma warning disable CS8618

public class ConfigureAccountAsRetailerCommandShould
{
    [Test]
    public async Task Create_Retailer_For_Specified_AccountIdentifier()
    {
        var (context, handler) = InitHandler();
        var accountIdentifier = AccountId.New();
        var command = GetCommand(accountIdentifier);

        var result = await handler.Handle(command, CancellationToken.None);

        var retailer = context.Retailers.SingleOrDefault(s => s.AccountIdentifier == accountIdentifier);
        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(retailer);
    }

    [Test]
    public async Task Fail_To_Create_Retailer_If_AccountIdentifier_Already_Registered()
    {
        var (context, handler) = InitHandler();
        var accountIdentifier = AccountId.New();
        var email = new EmailAddress("existing@test.com");
        context.Retailers.Add(new Retailer(new TradeName("trade"), email, new PhoneNumber("0664566565"),
            new Legal(new CorporateName("le"), new Siret("15932477173006"), new LegalAddress("", null, "", "")), null, accountIdentifier));
        await context.SaveChangesAsync();
        var command = GetCommand(accountIdentifier);
        
        var result = await handler.Handle(command, CancellationToken.None);

        var retailer = context.Retailers.Single(s => s.AccountIdentifier == accountIdentifier);
        Assert.IsTrue(result.IsFailure);
        Assert.IsNotNull(retailer);
        Assert.AreEqual(email, retailer.Email);
    }

    private (AppDbContext, ConfigureAccountAsRetailerHandler) InitHandler()
    {
        var (context, uow, logger) = DependencyHelpers.InitDependencies<ConfigureAccountAsRetailerHandler>();
        var handler = new ConfigureAccountAsRetailerHandler(uow, new ValidateRetailerRegistration(context));
        
        return (context, handler);
    }

    private static ConfigureAccountAsRetailerCommand GetCommand(AccountId accountIdentifier)
    {
        var address = new AddressDto("street", null, "74540", "city");
        var command = new ConfigureAccountAsRetailerCommand("TradeName", "CorporateName", "15932477173006", "test@test.com",
            "0654653221", address, address, accountIdentifier);
        return command;
    }
}
#pragma warning restore CS8767
#pragma warning restore CS8618
