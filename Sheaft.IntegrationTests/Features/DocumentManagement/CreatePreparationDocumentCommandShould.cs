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
        var result = await handler.Handle(new CreatePreparationDocumentCommand(new List<OrderId>(), SupplierId.New()),
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
        var orderIdentifiers = new List<OrderId> {OrderId.New(), OrderId.New()};

        var result = await handler.Handle(new CreatePreparationDocumentCommand(orderIdentifiers, SupplierId.New()),
            CancellationToken.None);

        Assert.IsTrue(result.IsSuccess);
        var document = context.Documents.Single(d => d.Identifier == new DocumentId(result.Value));
        Assert.IsNotNull(document);
        Assert.AreEqual(2,
            document.GetParams<PreparationDocumentParams>(new DocumentParamsHandler()).OrderIdentifiers.Count());
    }

    private (AppDbContext, CreatePreparationDocumentHandler) InitHandler()
    {
        var (context, uow, logger) = DependencyHelpers.InitDependencies<CreatePreparationDocumentHandler>();
        var handler = new CreatePreparationDocumentHandler(uow, new DocumentParamsHandler());

        return (context, handler);
    }
}