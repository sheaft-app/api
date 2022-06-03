using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.ProductManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.ProductManagement;

#pragma warning disable CS8604
[Route(Routes.RETURNABLES)]
public class GetReturnable : Feature
{
    public GetReturnable(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// Retrieve returnable with id
    /// </summary>
    [HttpGet("{id}", Name = nameof(GetReturnable))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ReturnableDto>> Get(string id, CancellationToken token)
    {
        var result = await Mediator.Query(new GetReturnableQuery(new ReturnableId(id)), token);
        return HandleCommandResult(result);
    }
}