using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.AccountManagement;
using Sheaft.Application.SupplierManagement;

namespace Sheaft.Web.Api.SupplierManagement;

[Route(Routes.SUPPLIERS)]
public class UpdateSupplier : Feature
{
    public UpdateSupplier(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// Update current user supplier profile
    /// </summary>
    [HttpPut("{id}", Name = nameof(UpdateSupplier))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Post([FromRoute] string id, [FromBody] SupplierInfoRequest data, CancellationToken token)
    {
        var result = await Mediator.Execute(data.Adapt<UpdateSupplierCommand>(), token);
        return HandleCommandResult(result);
    }
}