using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Sheaft.Application.AgreementManagement;
using Sheaft.Domain;
using Sheaft.Domain.AgreementManagement;
using Sheaft.Domain.ProductManagement;
using Sheaft.Domain.SupplierManagement;
using Sheaft.Infrastructure.AgreementManagement;
using Sheaft.Infrastructure.Persistence;
using Sheaft.IntegrationTests.Helpers;

namespace Sheaft.IntegrationTests.AgreementManagement;

#pragma warning disable CS8767
#pragma warning disable CS8618

public class ProposeAgreementToSupplierCommandShould
{
    [Test]
    public async Task Create_Agreement_Between_Customer_And_Supplier()
    {
        var (_, supplier, customer, context, handler) = InitHandler();
        var command = GetCommand(supplier.Identifier, customer.AccountIdentifier);

        var result = await handler.Handle(command, CancellationToken.None);

        var agreement = context.Agreements.Single(a => a.Identifier == new AgreementId(result.Value));
        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(agreement);
        Assert.AreEqual(AgreementOwner.Customer, agreement.Owner);
        Assert.AreEqual(supplier.Identifier, agreement.SupplierIdentifier);
        Assert.AreEqual(customer.Identifier, agreement.CustomerIdentifier);
    }
    
    [Test]
    public async Task Fail_If_Customer_Have_Already_A_Pending_Or_Active_Agreement()
    {
        var (catalog, supplier, customer, context, handler) = InitHandler();
        var command = GetCommand(supplier.Identifier, customer.AccountIdentifier);

        context.Add(Agreement.CreateCustomerAgreement(supplier.Identifier, customer.Identifier, catalog.Identifier));
        context.SaveChanges();

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual(ErrorKind.BadRequest, result.Error.Kind);
        Assert.AreEqual("agreement.already.exists", result.Error.Code);
    }

    private (Catalog, Supplier, Customer, AppDbContext, ProposeAgreementToSupplierHandler) InitHandler()
    {
        var (context, uow, logger) = DependencyHelpers.InitDependencies<ProposeAgreementToSupplierHandler>();

        var supplier = DataHelpers.GetDefaultSupplier(AccountId.New());
        var customer = DataHelpers.GetDefaultCustomer(AccountId.New());
        var catalog = Catalog.CreateDefaultCatalog(supplier.Identifier);
        
        context.Add(supplier);
        context.Add(customer);
        context.Add(catalog);
        context.SaveChanges();
        
        var handler = new ProposeAgreementToSupplierHandler(uow, new ValidateAgreementProposal(context));
        
        return (catalog, supplier, customer, context, handler);
    }

    private static ProposeAgreementToSupplierCommand GetCommand(SupplierId supplierIdentifier, AccountId customerAccountIdentifier)
    {
        var command = new ProposeAgreementToSupplierCommand(supplierIdentifier, customerAccountIdentifier);
        return command;
    }
}
#pragma warning restore CS8767
#pragma warning restore CS8618
