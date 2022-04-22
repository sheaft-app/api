using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Sheaft.Application.DocumentManagement;
using Sheaft.Domain;
using Sheaft.Domain.DocumentManagement;
using Sheaft.Infrastructure.DocumentManagement;
using Sheaft.Infrastructure.Persistence;
using Sheaft.IntegrationTests.Helpers;

namespace Sheaft.IntegrationTests.DocumentManagement;

public class CreatePreparationDocumentCommandShould
{
    [Test]
    public async Task Insert_Document_Waiting_For_Processing()
    {
        var (context, handler) = InitHandler();
        var supplier = context.Suppliers.Single();
        var orderIdentifiers = context.Orders.Select(o => o.Identifier).ToList();
        
        var result = await handler.Handle(new CreatePreparationDocumentCommand(orderIdentifiers, supplier.Identifier, true),
            CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        var document = context.Documents.Single(d => d.Identifier == new DocumentId(result.Value));
        Assert.IsNotNull(document);
        Assert.AreEqual(DocumentCategory.Orders, document.Category);
        Assert.AreEqual(DocumentKind.Preparation, document.Kind);
        Assert.AreEqual(DocumentStatus.Waiting, document.Status);
    }

    [Test]
    public async Task Insert_Document_And_Retrieve_Params()
    {
        var (context, handler) = InitHandler();
        var supplier = context.Suppliers.Single();
        var orderIdentifiers = context.Orders.Select(o => o.Identifier).ToList();

        var result = await handler.Handle(new CreatePreparationDocumentCommand(orderIdentifiers, supplier.Identifier, true),
            CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        var document = context.Documents.Single(d => d.Identifier == new DocumentId(result.Value));
        Assert.IsNotNull(document);
        Assert.AreEqual(2,
            document.GetParams<PreparationDocumentParams>(new DocumentParamsHandler()).OrderIdentifiers.Count());
    }

    [Test]
    public async Task Fail_If_Order_Not_Found()
    {
        var (context, handler) = InitHandler();
        var orderIdentifiers = new List<OrderId> {OrderId.New()};

        var result = await handler.Handle(new CreatePreparationDocumentCommand(orderIdentifiers, SupplierId.New(), true),
            CancellationToken.None);

        Assert.IsTrue(result.IsFailure);
    }

    [Test]
    public async Task Fail_If_Order_Not_Accepted_With_AutoAcceptOrders_False()
    {
        var (context, handler) = InitHandler(false);
        var supplier = context.Suppliers.Single();
        var orderIdentifiers = context.Orders.Select(o => o.Identifier).ToList();

        var result = await handler.Handle(new CreatePreparationDocumentCommand(orderIdentifiers, supplier.Identifier),
            CancellationToken.None);

        Assert.IsTrue(result.IsFailure);
        Assert.AreEqual(ErrorKind.BadRequest, result.Error.Kind);
        Assert.AreEqual("document.preparation.requires.accepted.orders", result.Error.Code);
    }
    
    [Test]
    public async Task Switch_Pending_Orders_To_Accepted_If_AutoAccept_True()
    {
        var (context, handler) = InitHandler(false);
        var supplier = context.Suppliers.Single();
        var orderIdentifiers = context.Orders.Select(o => o.Identifier).ToList();
        
        var result = await handler.Handle(new CreatePreparationDocumentCommand(orderIdentifiers, supplier.Identifier, true),
            CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        var orders = context.Orders.ToList();
        Assert.IsTrue(orders.All(o => o.Status == OrderStatus.Accepted));
    }

    private (AppDbContext, CreatePreparationDocumentHandler) InitHandler(bool accept = true)
    {
        var (context, uow, logger) = DependencyHelpers.InitDependencies<CreatePreparationDocumentHandler>();
        var handler = new CreatePreparationDocumentHandler(uow, new DocumentParamsHandler());
        var supplierId = AccountId.New();
        var customerId = AccountId.New();

        var agreements = new Dictionary<AccountId, DeliveryDay> {{customerId, new DeliveryDay(DayOfWeek.Friday)}};
        var supplierProducts = new Dictionary<string, int> {{"001", 2000}, {"002", 3500}};

        DataHelpers.InitContext(context,
            new List<AccountId> {customerId},
            new Dictionary<AccountId, Dictionary<AccountId, DeliveryDay>> {{supplierId, agreements}},
            new Dictionary<AccountId, Dictionary<string, int>> {{supplierId, supplierProducts}});
        
        var supplier = context.Suppliers.First();
        var customer = context.Customers.First();

        var order1 = DataHelpers.CreateOrderWithLines(supplier, customer, false, context.Products.ToList());
        if(accept)
            order1.Accept();
        
        context.Add(order1);
        
        var order2 = DataHelpers.CreateOrderWithLines(supplier, customer, false, context.Products.ToList());
        if(accept)
            order2.Accept();
        
        context.Add(order2);
        context.SaveChanges();
        
        return (context, handler);
    }
}