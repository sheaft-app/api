using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.BatchManagement;
using Sheaft.Application.ProductManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.BatchManagement;

#pragma warning disable CS8604
[Route(Routes.SUPPLIER_BATCHES)]
public class CreateBatch : Feature
{
    public CreateBatch(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// Create a batch with specified info
    /// </summary>
    [HttpPost("", Name = nameof(CreateBatch))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> Post(string supplierId, CreateBatchRequest data, CancellationToken token)
    {
        var result = await Mediator.Execute(new CreateBatchCommand(data.Number, data.DateKind,  data.ExpirationDate, data.ProductionDate, new SupplierId(supplierId)), token);
        return HandleCommandResult(result);
    }
}

public record CreateBatchRequest(string Number, BatchDateKind DateKind, DateTime ExpirationDate, DateTime? ProductionDate);
