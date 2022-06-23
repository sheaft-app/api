using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Sheaft.Application.AgreementManagement;
using Sheaft.Domain;
using Sheaft.Domain.AgreementManagement;
using Sheaft.Domain.CustomerManagement;
using Sheaft.Domain.ProductManagement;
using Sheaft.Domain.SupplierManagement;
using Sheaft.Infrastructure.AccountManagement;
using Sheaft.Infrastructure.AgreementManagement;
using Sheaft.Infrastructure.Persistence;
using Sheaft.UnitTests.Helpers;

namespace Sheaft.UnitTests.AgreementManagement;

#pragma warning disable CS8767
#pragma warning disable CS8618

public class ProposeAgreementToSupplierCommandShould
{
    [Test]
    public async Task Create_Agreement_With_Customer_As_Owner()
    {
        var (_, supplier, customer, context, handler) = InitHandler();
        var command = GetCommand(supplier.Id, customer.AccountId);

        var result = await handler.Handle(command, CancellationToken.None);

        var agreement = context.Agreements.Single(a => a.Id == new AgreementId(result.Value));
        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(agreement);
        Assert.AreEqual(AgreementOwner.Customer, agreement.Owner);
    }
    
    [Test]
    public async Task Fail_If_Customer_Has_Already_A_Pending_Or_Active_Agreement_For_Supplier()
    {
        var (catalog, supplier, customer, context, handler) = InitHandler();
        var command = GetCommand(supplier.Id, customer.AccountId);

        context.Add(Agreement.CreateAndSendAgreementToSupplier(supplier.Id, customer.Id, catalog.Id));
        context.SaveChanges();

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual(ErrorKind.BadRequest, result.Error.Kind);
        Assert.AreEqual("agreement.already.exists", result.Error.Code);
    }

    private (Catalog, Supplier, Customer, AppDbContext, ProposeAgreementToSupplierHandler) InitHandler()
    {
        var (context, uow, logger) = DependencyHelpers.InitDependencies<ProposeAgreementToSupplierHandler>();

        var supplierAcct  = DataHelpers.GetDefaultAccount(new PasswordHasher("super_password"), $"{Guid.NewGuid():N}@test.com");
        var customerAcct  = DataHelpers.GetDefaultAccount(new PasswordHasher("super_password"), $"{Guid.NewGuid():N}@test.com");
        
        var supplier = DataHelpers.GetDefaultSupplier(supplierAcct.Id);
        var customer = DataHelpers.GetDefaultCustomer(customerAcct.Id);
        
        var catalog = Catalog.CreateDefaultCatalog(supplier.Id);

        context.Add(customerAcct);
        context.Add(supplierAcct);
        context.Add(supplier);
        context.Add(customer);
        context.Add(catalog);
        
        context.SaveChanges();
        
        var handler = new ProposeAgreementToSupplierHandler(uow, new ValidateAgreementProposal(context));
        
        return (catalog, supplier, customer, context, handler);
    }

    private static ProposeAgreementToSupplierCommand GetCommand(SupplierId supplierIdentifier, AccountId customerAccountIdentifier)
    {
        var command = new ProposeAgreementToSupplierCommand(supplierIdentifier);
        command.SetRequestUser(new RequestUser(true, ProfileKind.Customer, customerAccountIdentifier));
        return command;
    }
}
#pragma warning restore CS8767
#pragma warning restore CS8618
