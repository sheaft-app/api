using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Sheaft.Application.Models;
using Sheaft.Application.RetailerManagement;
using Sheaft.Domain;
using Sheaft.Domain.RetailerManagement;
using Sheaft.Infrastructure.Persistence;
using Sheaft.IntegrationTests.Helpers;

namespace Sheaft.IntegrationTests.RetailerManagement;

#pragma warning disable CS8767
#pragma warning disable CS8618

public class UpdateRetailerCommandShould
{
    [Test]
    public async Task Update_Retailer_Information()
    {
        var (retailerId, context, handler) = InitHandler();
        var command = GetCommand(retailerId);

        var result = await handler.Handle(command, CancellationToken.None);

        var retailer = context.Retailers.Single(s => s.Identifier == retailerId);
        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(retailer);
        Assert.AreEqual("TradeName", retailer.TradeName.Value);
    }

    private (RetailerId, AppDbContext, UpdateRetailerHandler) InitHandler()
    {
        var (context, uow, _) = DependencyHelpers.InitDependencies<UpdateRetailerHandler>();
        var handler = new UpdateRetailerHandler(uow);

        var retailer = new Retailer(new TradeName("trade"), new EmailAddress("test@est.com"),
            new PhoneNumber("0664566565"),
            new Legal(new CorporateName("le"), new Siret("15932477173006"), new LegalAddress("", null, "", "")), null, AccountId.New());
        
        context.Retailers.Add(retailer);
        context.SaveChanges();
        
        return (retailer.Identifier, context, handler);
    }

    private static UpdateRetailerCommand GetCommand(RetailerId retailerIdentifier)
    {
        var address = new AddressDto("street", null, "74540", "city");
        var command = new UpdateRetailerCommand(retailerIdentifier, "TradeName", "CorporateName", "15932477173006", "test@test.com",
            "0654653221", address, address);
        return command;
    }
}
#pragma warning restore CS8767
#pragma warning restore CS8618
