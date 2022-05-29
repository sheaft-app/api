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
using Sheaft.Infrastructure.AccountManagement;
using Sheaft.Infrastructure.CustomerManagement;
using Sheaft.Infrastructure.DocumentManagement;
using Sheaft.Infrastructure.OrderManagement;
using Sheaft.Infrastructure.Persistence;
using Sheaft.UnitTests.Helpers;

namespace Sheaft.UnitTests.DocumentManagement;

public class ProcessDocumentCommandShould
{
    [Test]
    public async Task Set_Document_As_Done_And_Set_Url()
    {
        var (documentId, context, handler) = InitHandler();
        var result = await handler.Handle(new ProcessDocumentCommand(documentId),
            CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        var document = context.Documents.Single(d => d.Id == documentId);
        Assert.AreEqual(DocumentStatus.Done, document.Status);
    }

    [Test]
    public async Task Set_Status_As_InError_And_ErrorMessage()
    {
        var (documentId, context, handler) = InitHandler(true);
        var result = await handler.Handle(new ProcessDocumentCommand(documentId),
            CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        var document = context.Documents.Single(d => d.Id == documentId);
        Assert.AreEqual(DocumentStatus.InError, document.Status);
        Assert.AreEqual("file.error", document.ErrorMessage);
    }

    private (DocumentId, AppDbContext, ProcessDocumentHandler) InitHandler(bool fileReturnError = false)
    {
        var (context, uow, logger) = DependencyHelpers.InitDependencies<ProcessDocumentHandler>();

        var fileGeneratorMocked = new Mock<IPreparationFileGenerator>();
        var fileProviderMocked = new Mock<IFileStorage>();
        
        var fileResult = Result.Success();
        if (fileReturnError)
            fileResult = Result.Failure(ErrorKind.Unexpected, "file.error");

        fileGeneratorMocked.Setup(f => f.Generate(It.IsAny<PreparationDocumentData>(), CancellationToken.None))
            .Returns(Task.FromResult(Result.Success(Array.Empty<byte>())));
        
        fileProviderMocked.Setup(f => f.SaveDocument(It.IsAny<Document>(), It.IsAny<byte[]>(), CancellationToken.None))
            .Returns(Task.FromResult(fileResult));

        var handler = new ProcessDocumentHandler(uow,
            new DocumentProcessorFactory(
                new DocumentRepository(context),
                new OrderRepository(context),
                new CustomerRepository(context),
                new DocumentParamsHandler(),
                fileGeneratorMocked.Object,
                fileProviderMocked.Object));
        
        var supplierAccount  = DataHelpers.GetDefaultAccount(new PasswordHasher("super_password"), $"{Guid.NewGuid():N}@test.com");
        context.Add(supplierAccount);

        var customerAccount  = DataHelpers.GetDefaultAccount(new PasswordHasher("super_password"), $"{Guid.NewGuid():N}@test.com");
        context.Add(customerAccount);

        var agreements = new Dictionary<AccountId, DeliveryDay> {{customerAccount.Id, new DeliveryDay(DayOfWeek.Friday)}};
        var supplierProducts = new Dictionary<string, int> {{"001", 2000}, {"002", 3500}};

        DataHelpers.InitContext(context,
            new List<AccountId> {customerAccount.Id},
            new Dictionary<AccountId, Dictionary<AccountId, DeliveryDay>> {{supplierAccount.Id, agreements}},
            new Dictionary<AccountId, Dictionary<string, int>> {{supplierAccount.Id, supplierProducts}});
        
        var supplier = context.Suppliers.First();
        var customer = context.Customers.First();

        var order = DataHelpers.CreateOrderWithLines(supplier, customer, false, context.Products.ToList());
        order.Accept();
        context.Add(order);
        
        var orders = new List<OrderId> {order.Id};

        var document =
            Document.CreatePreparationDocument(new DocumentName("test"), new DocumentParamsHandler(), orders, new OwnerId(supplier.Id));
        
        context.Add(document);
        context.SaveChanges();

        return (document.Id, context, handler);
    }
}