using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.AgreementManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.SupplierManagement;

#pragma warning disable CS8604
[Route(Routes.SUPPLIERS)]
public class ProposeAgreementToSupplier : Feature
{
    public ProposeAgreementToSupplier(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// Send an agreement to supplier
    /// </summary>
    [HttpPost("{supplierId}/agreement", Name = nameof(ProposeAgreementToSupplier))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> Post(string supplierId, CancellationToken token)
    {
        var result = await Mediator.Execute(new ProposeAgreementToSupplierCommand(new SupplierId(supplierId), CurrentAccountId), token);
        return HandleCommandResult(result);
    }
}
