using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.DocumentManagement;
using Sheaft.Application.OrderManagement;
using Sheaft.Domain;

namespace Sheaft.Api.DocumentManagement;

[Route(Routes.DOCUMENTS)]
public class DownloadDocument : Feature
{
    public DownloadDocument(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    [HttpPost("{id}")]
    public async Task<ActionResult<string>> Post([FromRoute] string id, CancellationToken token)
    {
        var result = await Mediator.Execute(new DownloadDocumentCommand(new DocumentId(id)), token);
        return HandleCommandResult(result);
    }
}
