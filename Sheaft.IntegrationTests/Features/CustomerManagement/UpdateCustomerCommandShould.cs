using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Sheaft.Application.Models;
using Sheaft.Application.CustomerManagement;
using Sheaft.Domain;
using Sheaft.Infrastructure.Persistence;
using Sheaft.IntegrationTests.Helpers;

namespace Sheaft.IntegrationTests.CustomerManagement;

#pragma warning disable CS8767
#pragma warning disable CS8618

public class UpdateCustomerCommandShould
{
    [Test]
    public async Task Update_Customer_Information()
    {
        var (customerId, context, handler) = InitHandler();
        var command = GetCommand(customerId);

        var result = await handler.Handle(command, CancellationToken.None);

        var customer = context.Customers.Single(s => s.Identifier == customerId);
        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(customer);
        Assert.AreEqual("TradeName", customer.TradeName.Value);
    }

    private (CustomerId, AppDbContext, UpdateCustomerHandler) InitHandler()
    {
        var (context, uow, _) = DependencyHelpers.InitDependencies<UpdateCustomerHandler>();
        var handler = new UpdateCustomerHandler(uow);

        var customer = new Customer(new TradeName("trade"), new EmailAddress("test@est.com"),
            new PhoneNumber("0664566565"),
            new Legal(new CorporateName("le"), new Siret("15932477173006"), new LegalAddress("", null, "", "")), AccountId.New());
        
        context.Customers.Add(customer);
        context.SaveChanges();
        
        return (customer.Identifier, context, handler);
    }

    private static UpdateCustomerCommand GetCommand(CustomerId customerIdentifier)
    {
        var address = new AddressDto("street", null, "74540", "city");
        var namedAddress = new NamedAddressDto("ee", "tys@tese.com", "street", null, "74540", "city");
        var command = new UpdateCustomerCommand(customerIdentifier, "TradeName", "CorporateName", "15932477173006", "test@test.com",
            "0654653221", address, namedAddress, namedAddress);
        return command;
    }
}
#pragma warning restore CS8767
#pragma warning restore CS8618
