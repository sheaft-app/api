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
using Sheaft.Infrastructure.DocumentManagement;
using Sheaft.Infrastructure.Persistence;
using Sheaft.UnitTests.Helpers;

namespace Sheaft.UnitTests.DocumentManagement;

public class DownloadDocumentCommandShould
{
    private const string _expectedUrl = "https://myurl/document/id";
    
    [Test]
    public async Task Generate_Url()
    {
        var (documentId, context, handler) = InitHandler();
        
        var result = await handler.Handle(new DownloadDocumentCommand(documentId), CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(_expectedUrl, result.Value);
    }

    private (DocumentId, AppDbContext, DownloadDocumentHandler) InitHandler()
    {
        var (context, uow, logger) = DependencyHelpers.InitDependencies<DownloadDocumentHandler>();

        var fileStorageMocked = new Mock<IFileStorage>();
        fileStorageMocked.Setup(f => f.DownloadDocument(It.IsAny<Document>(), CancellationToken.None))
            .Returns(Task.FromResult(Result.Success(_expectedUrl)));

        var handler = new DownloadDocumentHandler(uow, fileStorageMocked.Object);

        var document = Document.CreatePreparationDocument(new DocumentName("tests"), new DocumentParamsHandler(), 
            new List<OrderId>(), OwnerId.New());
        
        context.Add(document);
        context.SaveChanges();
        
        return (document.Id, context, handler);
    }
}