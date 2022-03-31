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

public class ProposeAgreementToCustomerCommandShould
{
    [Test]
    public async Task Create_Agreement_Between_Supplier_And_Customer()
    {
        var (supplier, customer, context, handler) = InitHandler();
        var command = GetCommand(customer.Identifier, supplier.AccountIdentifier, new List<DayOfWeek> {DayOfWeek.Monday, DayOfWeek.Friday}, 24);

        var result = await handler.Handle(command, CancellationToken.None);

        var agreement = context.Agreements.Single(a => a.Identifier == new AgreementId(result.Value));
        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(agreement);
        Assert.AreEqual(supplier.Identifier, agreement.SupplierIdentifier);
        Assert.AreEqual(customer.Identifier, agreement.CustomerIdentifier);
        Assert.AreEqual(ProfileKind.Supplier, agreement.Owner);
        Assert.AreEqual(2, agreement.DeliveryDays.Count);
        Assert.AreEqual(24, agreement.OrderDelayInHoursBeforeDeliveryDay);
    }

    private (Supplier, Customer, AppDbContext, ProposeAgreementToCustomerHandler) InitHandler()
    {
        var (context, uow, logger) = DependencyHelpers.InitDependencies<ProposeAgreementToCustomerHandler>();

        var supplier = DataHelpers.GetDefaultSupplier(AccountId.New());
        var customer = DataHelpers.GetDefaultCustomer(AccountId.New());
        var catalog = Catalog.CreateDefaultCatalog(supplier.Identifier);
        context.Add(supplier);
        context.Add(customer);
        context.Add(catalog);
        
        context.SaveChanges();
        
        var handler = new ProposeAgreementToCustomerHandler(uow);
        
        return (supplier, customer, context, handler);
    }

    private static ProposeAgreementToCustomerCommand GetCommand(CustomerId customerIdentifier, AccountId supplierAccountIdentifier, List<DayOfWeek> deliveryDays, int orderDelayInHoursBeforeDeliveryDay)
    {
        var command = new ProposeAgreementToCustomerCommand(customerIdentifier, deliveryDays, orderDelayInHoursBeforeDeliveryDay, supplierAccountIdentifier);
        return command;
    }
}
#pragma warning restore CS8767
#pragma warning restore CS8618
