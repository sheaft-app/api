using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Sheaft.Application.Models;
using Sheaft.Application.CustomerManagement;
using Sheaft.Domain;
using Sheaft.Domain.CustomerManagement;
using Sheaft.Infrastructure.AccountManagement;
using Sheaft.Infrastructure.Persistence;
using Sheaft.Infrastructure.CustomerManagement;
using Sheaft.UnitTests.Helpers;

namespace Sheaft.UnitTests.CustomerManagement;

#pragma warning disable CS8767
#pragma warning disable CS8618

public class ConfigureAccountAsCustomerCommandShould
{
    [Test]
    public async Task Create_Customer_For_Specified_AccountIdentifier()
    {
        var (accountIdentifier, context, handler) = InitHandler();
        var command = GetCommand(accountIdentifier);

        var result = await handler.Handle(command, CancellationToken.None);

        var customer = context.Customers.SingleOrDefault(s => s.AccountId == accountIdentifier);
        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(customer);
    }

    [Test]
    public async Task Fail_To_Create_Customer_If_AccountIdentifier_Already_Registered()
    {
        var (accountIdentifier, context, handler) = InitHandler();
        var email = new EmailAddress("existing@test.com");
        context.Customers.Add(new Customer(new TradeName("trade"), email, new PhoneNumber("0664566565"),
            new Legal(new CorporateName("le"), new Siret("15932477173006"), new LegalAddress("", null, "", "")), accountIdentifier));
        await context.SaveChangesAsync();
        var command = GetCommand(accountIdentifier);
        
        var result = await handler.Handle(command, CancellationToken.None);

        var customer = context.Customers.Single(s => s.AccountId == accountIdentifier);
        Assert.IsTrue(result.IsFailure);
        Assert.IsNotNull(customer);
        Assert.AreEqual(email, customer.Email);
    }

    private (AccountId, AppDbContext, ConfigureAccountAsCustomerHandler) InitHandler()
    {
        var (context, uow, logger) = DependencyHelpers.InitDependencies<ConfigureAccountAsCustomerHandler>();
        var handler = new ConfigureAccountAsCustomerHandler(uow, new ValidateCustomerRegistration(context));
        
        var supplierAccount  = DataHelpers.GetDefaultAccount(new PasswordHasher("super_password"), $"{Guid.NewGuid():N}@test.com");
        context.Add(supplierAccount);
        context.SaveChanges();
        
        return (supplierAccount.Id, context, handler);
    }

    private static ConfigureAccountAsCustomerCommand GetCommand(AccountId accountIdentifier)
    {
        var address = new AddressDto("street", null, "74540", "city");
        var namedAddress = new NamedAddressDto("ee", "tys@tese.com", "street", null, "74540", "city");
        var command = new ConfigureAccountAsCustomerCommand("TradeName", "CorporateName", "15932477173006", "test@test.com",
            "0654653221", address, namedAddress, namedAddress, accountIdentifier);
        return command;
    }
}
#pragma warning restore CS8767
#pragma warning restore CS8618
