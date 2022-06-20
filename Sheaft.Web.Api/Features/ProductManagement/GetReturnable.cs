using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.ProductManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.ProductManagement;

#pragma warning disable CS8604
[Route(Routes.SUPPLIER_RETURNABLES)]
public class GetReturnable : Feature
{
    public GetReturnable(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// Retrieve returnable with id
    /// </summary>
    [HttpGet("{returnableId}", Name = nameof(GetReturnable))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ReturnableDto>> Get(string supplierId, string returnableId, CancellationToken token)
    {
        var result = await Mediator.Query(new GetReturnableQuery(new ReturnableId(returnableId), new SupplierId(supplierId)), token);
        return HandleCommandResult(result);
    }
}