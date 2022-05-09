using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.BatchManagement;
using Sheaft.Application.ProductManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.BatchManagement;

#pragma warning disable CS8604
[Route(Routes.BATCHES)]
public class CreateBatch : Feature
{
    public CreateBatch(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    [HttpPost("")]
    public async Task<ActionResult<string>> Post([FromBody] CreateBatchRequest data, CancellationToken token)
    {
        var result = await Mediator.Execute(new CreateBatchCommand(data.Number, data.DateKind,  data.Date, CurrentSupplierId), token);
        return HandleCommandResult(result);
    }
}

public record CreateBatchRequest(string Number, BatchDateKind DateKind, DateOnly Date);
