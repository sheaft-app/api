using Mapster;
using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.CustomerManagement;

namespace Sheaft.Web.Api.CustomerManagement;

[Route(Routes.CUSTOMERS)]
public class UpdateCustomer : Feature
{
    public UpdateCustomer(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// Update current user customer profile
    /// </summary>
    [HttpPut("{id}", Name = nameof(UpdateCustomer))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Post([FromRoute] string id, [FromBody] CustomerInfoRequest data, CancellationToken token)
    {
        var result = await Mediator.Execute(data.Adapt<UpdateCustomerCommand>(), token);
        return HandleCommandResult(result);
    }
}