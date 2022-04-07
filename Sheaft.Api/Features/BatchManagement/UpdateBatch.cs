using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.BatchManagement;
using Sheaft.Application.ProductManagement;
using Sheaft.Domain;

namespace Sheaft.Api.BatchManagement;

[Route(Routes.BATCHES)]
public class UpdateBatch : Feature
{
    public UpdateBatch(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Post([FromRoute] string id, [FromBody] UpdateBatchRequest data, CancellationToken token)
    {
        var result = await Mediator.Execute(new UpdateBatchCommand(new BatchId(id), data.Number, data.DateKind,  data.Date), token);
        return HandleCommandResult(result);
    }
}

public record UpdateBatchRequest(string Number, BatchDateKind DateKind, DateOnly Date);
