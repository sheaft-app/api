using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.DocumentManagement;
using Sheaft.Application.OrderManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.DocumentManagement;

[Route(Routes.DOCUMENTS)]
public class DownloadDocument : Feature
{
    public DownloadDocument(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// Generate a secured download link for document
    /// </summary>
    [HttpPost("{id}", Name = nameof(DownloadDocument))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> Post([FromRoute] string id, CancellationToken token)
    {
        var result = await Mediator.Execute(new DownloadDocumentCommand(new DocumentId(id)), token);
        return HandleCommandResult(result);
    }
}
