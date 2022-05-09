using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.DocumentManagement;
using Sheaft.Application.OrderManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.DocumentManagement;

[Route(Routes.DOCUMENTS)]
public class RemoveDocument : Feature
{
    public RemoveDocument(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Post([FromRoute] string id, CancellationToken token)
    {
        var result = await Mediator.Execute(new RemoveDocumentCommand(new DocumentId(id)), token);
        return HandleCommandResult(result);
    }
}
