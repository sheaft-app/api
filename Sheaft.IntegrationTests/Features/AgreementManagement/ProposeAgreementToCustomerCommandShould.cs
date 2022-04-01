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

public class ProposeAgreementToCustomerCommandShould
{
    [Test]
    public async Task Create_Agreement_Between_Supplier_And_Customer()
    {
        var (_, supplier, customer, context, handler) = InitHandler();
        var command = GetCommand(customer.Identifier, supplier.AccountIdentifier, new List<DayOfWeek> {DayOfWeek.Monday, DayOfWeek.Friday}, 24);

        var result = await handler.Handle(command, CancellationToken.None);
        Assert.IsTrue(result.IsSuccess);
        
        var agreement = context.Agreements.Single(a => a.Identifier == new AgreementId(result.Value));
        Assert.IsNotNull(agreement);
        Assert.AreEqual(supplier.Identifier, agreement.SupplierIdentifier);
        Assert.AreEqual(customer.Identifier, agreement.CustomerIdentifier);
        Assert.AreEqual(AgreementOwner.Supplier, agreement.Owner);
        Assert.AreEqual(2, agreement.DeliveryDays.Count);
        Assert.AreEqual(24, agreement.OrderDelayInHoursBeforeDeliveryDay);
    }
    
    [Test]
    public async Task Fail_If_Supplier_Have_Already_A_Pending_Or_Active_Agreement()
    {
        var (catalog, supplier, customer, context, handler) = InitHandler();
        var command = GetCommand(customer.Identifier, supplier.AccountIdentifier, new List<DayOfWeek> {DayOfWeek.Monday, DayOfWeek.Friday}, 24);

        context.Add(Agreement.CreateSupplierAgreement(supplier.Identifier, customer.Identifier, catalog.Identifier, new List<DeliveryDay>(), 0));
        context.SaveChanges();

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual(ErrorKind.BadRequest, result.Error.Kind);
        Assert.AreEqual("agreement.already.exists", result.Error.Code);
    }

    private (Catalog, Supplier, Customer, AppDbContext, ProposeAgreementToCustomerHandler) InitHandler()
    {
        var (context, uow, logger) = DependencyHelpers.InitDependencies<ProposeAgreementToCustomerHandler>();

        var supplier = DataHelpers.GetDefaultSupplier(AccountId.New());
        var customer = DataHelpers.GetDefaultCustomer(AccountId.New());
        var catalog = Catalog.CreateDefaultCatalog(supplier.Identifier);
        context.Add(supplier);
        context.Add(customer);
        context.Add(catalog);
        
        context.SaveChanges();
        
        var handler = new ProposeAgreementToCustomerHandler(uow, new ValidateAgreementProposal(context));
        
        return (catalog, supplier, customer, context, handler);
    }

    private static ProposeAgreementToCustomerCommand GetCommand(CustomerId customerIdentifier, AccountId supplierAccountIdentifier, List<DayOfWeek> deliveryDays, int orderDelayInHoursBeforeDeliveryDay)
    {
        var command = new ProposeAgreementToCustomerCommand(customerIdentifier, deliveryDays, orderDelayInHoursBeforeDeliveryDay, supplierAccountIdentifier);
        return command;
    }
}
#pragma warning restore CS8767
#pragma warning restore CS8618
