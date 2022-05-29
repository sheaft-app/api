using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
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

public class ProposeAgreementToCustomerCommandShould
{
    [Test]
    public async Task Create_Agreement_With_Supplier_As_Owner()
    {
        var (_, supplier, customer, context, handler) = InitHandler();
        var command = GetCommand(customer.Id, supplier.AccountId, new List<DayOfWeek> {DayOfWeek.Monday, DayOfWeek.Friday}, 24);

        var result = await handler.Handle(command, CancellationToken.None);
        Assert.IsTrue(result.IsSuccess);
        
        var agreement = context.Agreements.Single(a => a.Id == new AgreementId(result.Value));
        Assert.IsNotNull(agreement);
        Assert.AreEqual(supplier.Id, agreement.SupplierId);
        Assert.AreEqual(customer.Id, agreement.CustomerId);
        Assert.AreEqual(AgreementOwner.Supplier, agreement.Owner);
    }
    
    [Test]
    public async Task Fail_If_Supplier_Has_Already_A_Pending_Or_Active_Agreement_For_Customer()
    {
        var (catalog, supplier, customer, context, handler) = InitHandler();
        var command = GetCommand(customer.Id, supplier.AccountId, new List<DayOfWeek> {DayOfWeek.Monday, DayOfWeek.Friday}, 24);

        context.Add(Agreement.CreateAndSendAgreementToCustomer(supplier.Id, customer.Id, catalog.Id, new List<DeliveryDay>(), 0));
        context.SaveChanges();

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual(ErrorKind.BadRequest, result.Error.Kind);
        Assert.AreEqual("agreement.already.exists", result.Error.Code);
    }

    private (Catalog, Supplier, Customer, AppDbContext, ProposeAgreementToCustomerHandler) InitHandler()
    {
        var (context, uow, logger) = DependencyHelpers.InitDependencies<ProposeAgreementToCustomerHandler>();

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
