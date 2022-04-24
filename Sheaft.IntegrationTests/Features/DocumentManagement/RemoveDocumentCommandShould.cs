﻿using System;
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
using Sheaft.IntegrationTests.Helpers;

namespace Sheaft.IntegrationTests.DocumentManagement;

public class RemoveDocumentCommandShould
{
    [Test]
    public async Task Remove_Document_From_Database()
    {
        var (documentId, context, handler) = InitHandler();
        
        var result = await handler.Handle(new RemoveDocumentCommand(documentId), CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        var document = context.Documents.SingleOrDefault(d => d.Identifier == documentId);
        Assert.IsNull(document);
    }

    private (DocumentId, AppDbContext, RemoveDocumentHandler) InitHandler()
    {
        var (context, uow, logger) = DependencyHelpers.InitDependencies<RemoveDocumentHandler>();

        var fileStorageMocked = new Mock<IFileStorage>();
        fileStorageMocked.Setup(f => f.RemoveDocument(It.IsAny<Document>(), CancellationToken.None))
            .Returns(Task.FromResult(Result.Success()));

        var handler = new RemoveDocumentHandler(uow, fileStorageMocked.Object);

        var document = Document.CreatePreparationDocument(new DocumentName("tests"), new DocumentParamsHandler(), 
            new List<OrderId>(), SupplierId.New());
        
        context.Add(document);
        context.SaveChanges();
        
        return (document.Identifier, context, handler);
    }
}