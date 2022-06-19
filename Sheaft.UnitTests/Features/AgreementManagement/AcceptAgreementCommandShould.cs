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

public class AcceptAgreementCommandShould
{
    [Test]
    public async Task Set_As_Active_Agreement_Sent_By_Supplier()
    {
        var (supplier, customer, catalog, context, handler) = InitSupplierHandler();
        var agreement = Agreement.CreateAndSendAgreementToCustomer(supplier.Id, customer.Id,
            catalog.Id, new List<DeliveryDay>{new(DayOfWeek.Friday)}, 24);

        context.Add(agreement); 
        context.SaveChanges();
        
        var result = await handler.Handle(new AcceptSupplierAgreementCommand(agreement.Id), CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(AgreementStatus.Active, agreement.Status);
    }
    
    [Test]
    public async Task Set_As_Active_Agreement_Sent_By_Customer()
    {
        var (supplier, customer, catalog, context, handler) = InitCustomerHandler();
        
        var agreement = Agreement.CreateAndSendAgreementToSupplier(supplier.Id, customer.Id, catalog.Id);
        context.Add(agreement);
        context.SaveChanges();
        
        var result = await handler.Handle(new AcceptCustomerAgreementCommand(agreement.Id, new List<DayOfWeek>
            {
                DayOfWeek.Monday
            }, 24), CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(AgreementStatus.Active, agreement.Status);
    }
    
    
    [Test]
    public async Task Assign_DeliveryDay_Agreement_Sent_By_Customer()
    {
        var (supplier, customer, catalog, context, handler) = InitCustomerHandler();
        
        var agreement = Agreement.CreateAndSendAgreementToSupplier(supplier.Id, customer.Id, catalog.Id);
        context.Add(agreement);
        context.SaveChanges();
        
        var result = await handler.Handle(new AcceptCustomerAgreementCommand(agreement.Id, new List<DayOfWeek>
        {
            DayOfWeek.Monday
        }, 24), CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(DayOfWeek.Monday, agreement.DeliveryDays.First().Value);
    }
    
    [Test]
    public async Task Fail_If_Supplier_Accept_Agreement_From_Customer_Without_DeliveryDays()
    {
        var (supplier, customer, catalog, context, handler) = InitCustomerHandler();
        
        var agreement = Agreement.CreateAndSendAgreementToSupplier(supplier.Id, customer.Id, catalog.Id);
        context.Add(agreement);
        context.SaveChanges();
        
        var result = await handler.Handle(new AcceptCustomerAgreementCommand(agreement.Id, new List<DayOfWeek>(), 0), CancellationToken.None);

        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual(ErrorKind.BadRequest, result.Error.Kind);
    }
    
    [Test]
    public async Task Fail_To_Accept_Agreement_If_Not_In_Pending_Status()
    {
        var (supplier, customer, catalog, context, handler) = InitSupplierHandler();
        
        var agreement = Agreement.CreateAndSendAgreementToCustomer(supplier.Id, customer.Id,
            catalog.Id, new List<DeliveryDay>{new(DayOfWeek.Friday)}, 24);

        agreement.AcceptAgreement();
        
        context.Add(agreement); 
        context.SaveChanges();
        
        var result = await handler.Handle(new AcceptSupplierAgreementCommand(agreement.Id), CancellationToken.None);

        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual(ErrorKind.BadRequest, result.Error.Kind);
        Assert.AreEqual("agreement.accept.requires.pending", result.Error.Code);
    }

    private (Supplier, Customer, Catalog, AppDbContext, AcceptCustomerAgreementHandler) InitCustomerHandler()
    {
        var (context, uow, logger) = DependencyHelpers.InitDependencies<AcceptCustomerAgreementHandler>();
        var (supplier, customer, catalog) = InitContext(context);
        
        var handler = new AcceptCustomerAgreementHandler(uow);
        
        return (supplier, customer, catalog, context, handler);
    }

    private (Supplier, Customer, Catalog, AppDbContext, AcceptSupplierAgreementHandler) InitSupplierHandler()
    {
        var (context, uow, logger) = DependencyHelpers.InitDependencies<AcceptSupplierAgreementHandler>();
        var (supplier, customer, catalog) = InitContext(context);
        
        var handler = new AcceptSupplierAgreementHandler(uow);
        
        return (supplier, customer, catalog, context, handler);
    }

    private static (Supplier, Customer, Catalog) InitContext(AppDbContext context)
    {
        var supplierAcct =
            DataHelpers.GetDefaultAccount(new PasswordHasher("super_password"), $"{Guid.NewGuid():N}@test.com");
        var customerAcct =
            DataHelpers.GetDefaultAccount(new PasswordHasher("super_password"), $"{Guid.NewGuid():N}@test.com");

        var supplier = DataHelpers.GetDefaultSupplier(supplierAcct.Id);
        var customer = DataHelpers.GetDefaultCustomer(customerAcct.Id);

        var catalog = Catalog.CreateDefaultCatalog(supplier.Id);

        context.Add(customerAcct);
        context.Add(supplierAcct);
        context.Add(supplier);
        context.Add(customer);
        context.Add(catalog);
        
        context.SaveChanges();
        
        return (supplier, customer, catalog);
    }
}
#pragma warning restore CS8767
#pragma warning restore CS8618
