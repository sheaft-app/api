using Mapster;
using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.CustomerManagement;

namespace Sheaft.Api.CustomerManagement;

[Route(Routes.PROFILES)]
public class UpdateCustomer : Feature
{
    public UpdateCustomer(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    [HttpPut("customers/{id}")]
    public async Task<ActionResult> Post([FromRoute] string id, [FromBody] CustomerInfoRequest data, CancellationToken token)
    {
        var result = await Mediator.Execute(data.Adapt<UpdateCustomerCommand>(), token);
        return HandleCommandResult(result);
    }
}