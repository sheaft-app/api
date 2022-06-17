using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.AgreementManagement;
using Sheaft.Application.SupplierManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.SupplierManagement;

#pragma warning disable CS8604
[Route(Routes.SUPPLIERS)]
public class GetAvailableSupplier : Feature
{
    public GetAvailableSupplier(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// Retrieve supplier with id
    /// </summary>
    [HttpGet("{id}", Name = nameof(GetAvailableSupplier))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AvailableSupplierDto>> Get(string id, CancellationToken token)
    {
        var result = await Mediator.Query(new GetAvailableSupplierQuery(new SupplierId(id)), token);
        return HandleCommandResult(result);
    }
}