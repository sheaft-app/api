using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.AgreementManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.AgreementManagement;

#pragma warning disable CS8604
[Route(Routes.SUPPLIERS)]
public class ProposeAgreementToSupplier : Feature
{
    public ProposeAgreementToSupplier(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    [HttpPost("{id}/agreement")]
    public async Task<ActionResult<string>> Post([FromRoute] string id, CancellationToken token)
    {
        var result = await Mediator.Execute(new ProposeAgreementToSupplierCommand(new SupplierId(id), CurrentAccountId), token);
        return HandleCommandResult(result);
    }
}
