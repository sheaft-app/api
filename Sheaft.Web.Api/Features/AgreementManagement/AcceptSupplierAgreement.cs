using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.AgreementManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.AgreementManagement;

[Route(Routes.AGREEMENTS)]
public class AcceptSupplierAgreement : Feature
{
    public AcceptSupplierAgreement(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// Accept supplier agreement
    /// </summary>
    [HttpPut("{id}/accept/supplier", Name = nameof(AcceptSupplierAgreement))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Post([FromRoute] string id, CancellationToken token)
    {
        var result = await Mediator.Execute(new AcceptSupplierAgreementCommand(new AgreementId(id)), token);
        return HandleCommandResult(result);
    }
}