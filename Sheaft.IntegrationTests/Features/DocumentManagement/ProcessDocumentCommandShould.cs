using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Sheaft.Application.DocumentManagement;
using Sheaft.Domain;
using Sheaft.Domain.DocumentManagement;
using Sheaft.Infrastructure.CustomerManagement;
using Sheaft.Infrastructure.DocumentManagement;
using Sheaft.Infrastructure.OrderManagement;
using Sheaft.Infrastructure.Persistence;
using Sheaft.IntegrationTests.Helpers;

namespace Sheaft.IntegrationTests.DocumentManagement;

public class ProcessDocumentCommandShould
{
    private static string _urlFile = "https://test.com/file";

    [Test]
    public async Task Set_Document_As_Done_And_Set_Url()
    {
        var (documentId, context, handler) = InitHandler();
        var result = await handler.Handle(new ProcessDocumentCommand(documentId),
            CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        var document = context.Documents.Single(d => d.Identifier == documentId);
        Assert.AreEqual(DocumentStatus.Done, document.Status);
        Assert.AreEqual(_urlFile, document.Url);
    }

    [Test]
    public async Task Set_Status_As_InError_And_ErrorMessage()
    {
        var (documentId, context, handler) = InitHandler(true);
        var result = await handler.Handle(new ProcessDocumentCommand(documentId),
            CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        var document = context.Documents.Single(d => d.Identifier == documentId);
        Assert.AreEqual(DocumentStatus.InError, document.Status);
        Assert.AreEqual("file.error", document.ErrorMessage);
    }

    private (DocumentId, AppDbContext, ProcessDocumentHandler) InitHandler(bool fileReturnError = false)
    {
        var (context, uow, logger) = DependencyHelpers.InitDependencies<ProcessDocumentHandler>();

        var fileGeneratorMocked = new Mock<IPreparationFileGenerator>();
        var fileResult = Result.Success(_urlFile);
        if (fileReturnError)
            fileResult = Result.Failure<string>(ErrorKind.Unexpected, "file.error");

        fileGeneratorMocked.Setup(f => f.Generate(It.IsAny<PreparationDocumentData>(), CancellationToken.None))
            .Returns(Task.FromResult(fileResult));

        var handler = new ProcessDocumentHandler(uow,
            new DocumentProcessorFactory(
                new DocumentRepository(context),
                new OrderRepository(context),
                new CustomerRepository(context),
                new DocumentParamsHandler(),
                fileGeneratorMocked.Object));
        
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

        var order = DataHelpers.CreateOrderWithLines(supplier, customer, false, context.Products.ToList());
        order.Accept();
        context.Add(order);
        
        var orders = new List<OrderId> {order.Identifier};

        var document =
            Document.CreatePreparationDocument("test", new DocumentParamsHandler(), orders, supplier.Identifier);
        
        context.Add(document);
        context.SaveChanges();

        return (document.Identifier, context, handler);
    }
}