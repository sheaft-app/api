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
        
        var agreement = Agreement.CreateAndSendAgreementToCustomer(supplier.Identifier, customer.Identifier,
            catalog.Identifier, new List<DeliveryDay>{new(DayOfWeek.Friday)}, 24);

        agreement.Accept();
        context.Add(agreement); 
        context.SaveChanges();
        
        var result = await handler.Handle(new RevokeAgreementCommand(agreement.Identifier, "raison valable"), CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(AgreementStatus.Revoked, agreement.Status);
        Assert.AreEqual("raison valable", agreement.FailureReason);
    }
    
    [Test]
    public async Task Fail_To_Revoke_Agreement_If_Not_In_Active_Status()
    {
        var (supplier, customer, catalog, context, handler) = InitHandler();
        
        var agreement = Agreement.CreateAndSendAgreementToCustomer(supplier.Identifier, customer.Identifier,
            catalog.Identifier, new List<DeliveryDay>{new(DayOfWeek.Friday)}, 24);

        agreement.Refuse();
        
        context.Add(agreement); 
        context.SaveChanges();
        
        var result = await handler.Handle(new RevokeAgreementCommand(agreement.Identifier, "reason"), CancellationToken.None);

        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual(ErrorKind.BadRequest, result.Error.Kind);
        Assert.AreEqual("agreement.revoke.requires.active", result.Error.Code);
    }
    
    [Test]
    public async Task Fail_To_Revoke_Agreement_If_No_Reason_Provided()
    {
        var (supplier, customer, catalog, context, handler) = InitHandler();
        
        var agreement = Agreement.CreateAndSendAgreementToCustomer(supplier.Identifier, customer.Identifier,
            catalog.Identifier, new List<DeliveryDay>{new(DayOfWeek.Friday)}, 24);

        agreement.Accept();
        
        context.Add(agreement); 
        context.SaveChanges();
        
        var result = await handler.Handle(new RevokeAgreementCommand(agreement.Identifier, null), CancellationToken.None);

        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual(ErrorKind.BadRequest, result.Error.Kind);
        Assert.AreEqual("agreement.revoke.requires.reason", result.Error.Code);
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
