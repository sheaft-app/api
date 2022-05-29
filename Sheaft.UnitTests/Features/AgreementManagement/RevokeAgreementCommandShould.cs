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

public class RevokeAgreementCommandShould
{
    [Test]
    public async Task Revoke_Agreement_Between_Customer_And_Supplier()
    {
        var (supplier, customer, catalog, context, handler) = InitHandler();
        
        var agreement = Agreement.CreateAndSendAgreementToCustomer(supplier.Id, customer.Id,
            catalog.Id, new List<DeliveryDay>{new(DayOfWeek.Friday)}, 24);

        agreement.Accept();
        context.Add(agreement); 
        context.SaveChanges();
        
        var result = await handler.Handle(new RevokeAgreementCommand(agreement.Id, "raison valable"), CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(AgreementStatus.Revoked, agreement.Status);
        Assert.AreEqual("raison valable", agreement.FailureReason);
    }
    
    [Test]
    public async Task Fail_To_Revoke_Agreement_If_Not_In_Active_Status()
    {
        var (supplier, customer, catalog, context, handler) = InitHandler();
        
        var agreement = Agreement.CreateAndSendAgreementToCustomer(supplier.Id, customer.Id,
            catalog.Id, new List<DeliveryDay>{new(DayOfWeek.Friday)}, 24);

        agreement.Refuse();
        
        context.Add(agreement); 
        context.SaveChanges();
        
        var result = await handler.Handle(new RevokeAgreementCommand(agreement.Id, "reason"), CancellationToken.None);

        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual(ErrorKind.BadRequest, result.Error.Kind);
        Assert.AreEqual("agreement.revoke.requires.active", result.Error.Code);
    }
    
    [Test]
    public async Task Fail_To_Revoke_Agreement_If_No_Reason_Provided()
    {
        var (supplier, customer, catalog, context, handler) = InitHandler();
        
        var agreement = Agreement.CreateAndSendAgreementToCustomer(supplier.Id, customer.Id,
            catalog.Id, new List<DeliveryDay>{new(DayOfWeek.Friday)}, 24);

        agreement.Accept();
        
        context.Add(agreement); 
        context.SaveChanges();
        
        var result = await handler.Handle(new RevokeAgreementCommand(agreement.Id, null), CancellationToken.None);

        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual(ErrorKind.BadRequest, result.Error.Kind);
        Assert.AreEqual("agreement.revoke.requires.reason", result.Error.Code);
    }

    private (Supplier, Customer, Catalog, AppDbContext, RevokeAgreementHandler) InitHandler()
    {
        var (context, uow, logger) = DependencyHelpers.InitDependencies<RevokeAgreementHandler>();

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
        
        var handler = new RevokeAgreementHandler(uow);
        
        return (supplier, customer, catalog, context, handler);
    }
}
#pragma warning restore CS8767
#pragma warning restore CS8618
