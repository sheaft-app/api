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

public class AcceptAgreementFromCustomerCommandShould
{
    [Test]
    public async Task As_Customer_Accept_Agreement_From_Supplier()
    {
        var (supplier, customer, catalog, context, handler) = InitHandler();
        
        var agreement = Agreement.CreateSupplierAgreement(supplier.Identifier, customer.Identifier,
            catalog.Identifier, new List<DeliveryDay>{new(DayOfWeek.Friday)}, 24);

        context.Add(agreement); 
        context.SaveChanges();
        
        var result = await handler.Handle(new AcceptAgreementCommand(agreement.Identifier), CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(AgreementStatus.Active, agreement.Status);
    }
    
    [Test]
    public async Task As_Supplier_Accept_Agreement_From_Customer()
    {
        var (supplier, customer, catalog, context, handler) = InitHandler();
        
        var agreement = Agreement.CreateCustomerAgreement(supplier.Identifier, customer.Identifier, catalog.Identifier);
        context.Add(agreement);
        context.SaveChanges();
        
        var result = await handler.Handle(new AcceptAgreementCommand(agreement.Identifier, new List<DayOfWeek>
            {
                DayOfWeek.Monday
            }, 24), CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(AgreementStatus.Active, agreement.Status);
    }
    
    [Test]
    public async Task Fail_If_As_Supplier_Accept_Agreement_From_Customer_Without_DeliveryDays_Or_Delay()
    {
        var (supplier, customer, catalog, context, handler) = InitHandler();
        
        var agreement = Agreement.CreateCustomerAgreement(supplier.Identifier, customer.Identifier, catalog.Identifier);
        context.Add(agreement);
        context.SaveChanges();
        
        var result = await handler.Handle(new AcceptAgreementCommand(agreement.Identifier), CancellationToken.None);

        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual(ErrorKind.BadRequest, result.Error.Kind);
    }

    private (Supplier, Customer, Catalog, AppDbContext, AcceptAgreementHandler) InitHandler()
    {
        var (context, uow, logger) = DependencyHelpers.InitDependencies<AcceptAgreementHandler>();

        var supplier = DataHelpers.GetDefaultSupplier(AccountId.New());
        var customer = DataHelpers.GetDefaultCustomer(AccountId.New());
        var catalog = Catalog.CreateDefaultCatalog(supplier.Identifier);
        
        context.Add(supplier);
        context.Add(customer);
        context.Add(catalog);
        
        context.SaveChanges();
        
        var handler = new AcceptAgreementHandler(uow);
        
        return (supplier, customer, catalog, context, handler);
    }
}
#pragma warning restore CS8767
#pragma warning restore CS8618
