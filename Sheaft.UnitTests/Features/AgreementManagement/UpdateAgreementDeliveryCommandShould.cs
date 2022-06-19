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
using Sheaft.Infrastructure.Persistence;
using Sheaft.UnitTests.Helpers;

namespace Sheaft.UnitTests.AgreementManagement;

#pragma warning disable CS8767
#pragma warning disable CS8618

public class UpdateAgreementDeliveryCommandShould
{
    [Test]
    public async Task Update_Agreement_DeliveryDays_And_Delay()
    {
        var (supplier, customer, catalog, context, handler) = InitHandler();
        
        var agreement = Agreement.CreateAndSendAgreementToCustomer(supplier.Id, customer.Id,
            catalog.Id, new List<DeliveryDay>{new(DayOfWeek.Friday)}, 24);

        context.Add(agreement); 
        context.SaveChanges();
        
        var result = await handler.Handle(new UpdateAgreementDeliveryCommand(agreement.Id, new List<DayOfWeek>
            {
                DayOfWeek.Monday
            }, 12), CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        
        Assert.AreEqual(AgreementStatus.Pending, agreement.Status);
        Assert.AreEqual(DayOfWeek.Monday, agreement.DeliveryDays.First().Value);
        Assert.AreEqual(12, agreement.OrderDelayInHoursBeforeDeliveryDay);
    }
    
    [Test]
    public async Task Fail_If_Agreement_Not_Pending_Or_Active()
    {
        var (supplier, customer, catalog, context, handler) = InitHandler();
        
        var agreement = Agreement.CreateAndSendAgreementToCustomer(supplier.Id, customer.Id,
            catalog.Id, new List<DeliveryDay>{new(DayOfWeek.Friday)}, 24);

        agreement.Refuse("test");
        
        context.Add(agreement);
        context.SaveChanges();
        
        var result = await handler.Handle(new UpdateAgreementDeliveryCommand(agreement.Id, new List<DayOfWeek>()), CancellationToken.None);

        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual(ErrorKind.BadRequest, result.Error.Kind);
        Assert.AreEqual("agreement.delivery.requires.pending.or.active.status", result.Error.Code);
    }
    
    [Test]
    public async Task Fail_If_Agreement_No_Delivery_Days_Provided()
    {
        var (supplier, customer, catalog, context, handler) = InitHandler();
        
        var agreement = Agreement.CreateAndSendAgreementToCustomer(supplier.Id, customer.Id,
            catalog.Id, new List<DeliveryDay>{new(DayOfWeek.Friday)}, 24);
        
        context.Add(agreement);
        context.SaveChanges();
        
        var result = await handler.Handle(new UpdateAgreementDeliveryCommand(agreement.Id, new List<DayOfWeek>()), CancellationToken.None);

        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual(ErrorKind.BadRequest, result.Error.Kind);
        Assert.AreEqual("agreement.delivery.days.required", result.Error.Code);
    }

    private (Supplier, Customer, Catalog, AppDbContext, UpdateAgreementDeliveryHandler) InitHandler()
    {
        var (context, uow, logger) = DependencyHelpers.InitDependencies<UpdateAgreementDeliveryHandler>();

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
        
        var handler = new UpdateAgreementDeliveryHandler(uow);
        
        return (supplier, customer, catalog, context, handler);
    }
}
#pragma warning restore CS8767
#pragma warning restore CS8618
