using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Sheaft.Application.AgreementManagement;
using Sheaft.Domain;
using Sheaft.Domain.CustomerManagement;
using Sheaft.Domain.ProductManagement;
using Sheaft.Domain.SupplierManagement;
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
        var (supplier, customer, context, handler) = InitHandler();
        var command = GetCommand(supplier.Identifier, customer.AccountIdentifier);

        var result = await handler.Handle(command, CancellationToken.None);

        var agreement = context.Agreements.Single(a => a.Identifier == new AgreementId(result.Value));
        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(agreement);
        Assert.AreEqual(AgreementOwner.Customer, agreement.Owner);
        Assert.AreEqual(supplier.Identifier, agreement.SupplierIdentifier);
        Assert.AreEqual(customer.Identifier, agreement.CustomerIdentifier);
    }

    private (Supplier, Customer, AppDbContext, ProposeAgreementToSupplierHandler) InitHandler()
    {
        var (context, uow, logger) = DependencyHelpers.InitDependencies<ProposeAgreementToSupplierHandler>();

        var supplier = DataHelpers.GetDefaultSupplier(AccountId.New());
        var customer = DataHelpers.GetDefaultCustomer(AccountId.New());
        var catalog = Catalog.CreateDefaultCatalog(supplier.Identifier);
        context.Add(supplier);
        context.Add(customer);
        context.Add(catalog);
        context.SaveChanges();
        
        var handler = new ProposeAgreementToSupplierHandler(uow);
        
        return (supplier, customer, context, handler);
    }

    private static ProposeAgreementToSupplierCommand GetCommand(SupplierId supplierIdentifier, AccountId customerAccountIdentifier)
    {
        var command = new ProposeAgreementToSupplierCommand(supplierIdentifier, customerAccountIdentifier);
        return command;
    }
}
#pragma warning restore CS8767
#pragma warning restore CS8618
