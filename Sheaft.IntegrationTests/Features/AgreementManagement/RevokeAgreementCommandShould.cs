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
using Sheaft.Infrastructure.Persistence;
using Sheaft.IntegrationTests.Helpers;

namespace Sheaft.IntegrationTests.AgreementManagement;

#pragma warning disable CS8767
#pragma warning disable CS8618

public class RevokeAgreementCommandShould
{
    [Test]
    public async Task Revoke_Agreement_Between_Customer_And_Supplier()
    {
        var (supplier, customer, catalog, context, handler) = InitHandler();
        
        var agreement = Agreement.CreateSupplierAgreement(supplier.Identifier, customer.Identifier,
            catalog.Identifier, new List<DeliveryDay>{new(DayOfWeek.Friday)}, 24);

        agreement.Accept();
        context.Add(agreement); 
        context.SaveChanges();
        
        var result = await handler.Handle(new RevokeAgreementCommand(agreement.Identifier, "raison valable"), CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(AgreementStatus.Revoked, agreement.Status);
        Assert.AreEqual("raison valable", agreement.RevokedReason);
    }

    private (Supplier, Customer, Catalog, AppDbContext, RevokeAgreementHandler) InitHandler()
    {
        var (context, uow, logger) = DependencyHelpers.InitDependencies<RevokeAgreementHandler>();

        var supplier = DataHelpers.GetDefaultSupplier(AccountId.New());
        var customer = DataHelpers.GetDefaultCustomer(AccountId.New());
        var catalog = Catalog.CreateDefaultCatalog(supplier.Identifier);
        
        context.Add(supplier);
        context.Add(customer);
        context.Add(catalog);
        
        context.SaveChanges();
        
        var handler = new RevokeAgreementHandler(uow);
        
        return (supplier, customer, catalog, context, handler);
    }
}
#pragma warning restore CS8767
#pragma warning restore CS8618
