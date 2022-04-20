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
using Sheaft.IntegrationTests.Helpers;

namespace Sheaft.IntegrationTests.AgreementManagement;

#pragma warning disable CS8767
#pragma warning disable CS8618

public class RefuseAgreementCommandShould
{
    [Test]
    public async Task Set_Status_To_Refused()
    {
        var (supplier, customer, catalog, context, handler) = InitHandler();
        
        var agreement = Agreement.CreateAndSendAgreementToCustomer(supplier.Identifier, customer.Identifier,
            catalog.Identifier, new List<DeliveryDay>{new(DayOfWeek.Friday)}, 24);

        context.Add(agreement); 
        context.SaveChanges();
        
        var result = await handler.Handle(new RefuseAgreementCommand(agreement.Identifier), CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(AgreementStatus.Refused, agreement.Status);
    }
    
    [Test]
    public async Task Set_RefusedReason()
    {
        var (supplier, customer, catalog, context, handler) = InitHandler();
        
        var agreement = Agreement.CreateAndSendAgreementToCustomer(supplier.Identifier, customer.Identifier,
            catalog.Identifier, new List<DeliveryDay>{new(DayOfWeek.Friday)}, 24);

        context.Add(agreement); 
        context.SaveChanges();
        
        var result = await handler.Handle(new RefuseAgreementCommand(agreement.Identifier, "reason"), CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual("reason", agreement.FailureReason);
    }
    
    [Test]
    public async Task Fail_To_Refuse_Agreement_If_Not_In_Pending_Status()
    {
        var (supplier, customer, catalog, context, handler) = InitHandler();
        
        var agreement = Agreement.CreateAndSendAgreementToCustomer(supplier.Identifier, customer.Identifier,
            catalog.Identifier, new List<DeliveryDay>{new(DayOfWeek.Friday)}, 24);

        agreement.Accept();
        
        context.Add(agreement); 
        context.SaveChanges();
        
        var result = await handler.Handle(new RefuseAgreementCommand(agreement.Identifier), CancellationToken.None);

        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual(ErrorKind.BadRequest, result.Error.Kind);
        Assert.AreEqual("agreement.refuse.requires.pending", result.Error.Code);
    }

    private (Supplier, Customer, Catalog, AppDbContext, RefuseAgreementHandler) InitHandler()
    {
        var (context, uow, logger) = DependencyHelpers.InitDependencies<RefuseAgreementHandler>();

        var supplier = DataHelpers.GetDefaultSupplier(AccountId.New());
        var customer = DataHelpers.GetDefaultCustomer(AccountId.New());
        var catalog = Catalog.CreateDefaultCatalog(supplier.Identifier);
        
        context.Add(supplier);
        context.Add(customer);
        context.Add(catalog);
        
        context.SaveChanges();
        
        var handler = new RefuseAgreementHandler(uow);
        
        return (supplier, customer, catalog, context, handler);
    }
}
#pragma warning restore CS8767
#pragma warning restore CS8618
