using Mapster;
using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.RetailerManagement;

namespace Sheaft.Api.RetailerManagement;

[Route(Routes.PROFILES)]
public class UpdateRetailer : Feature
{
    public UpdateRetailer(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    [HttpPut("retailers/{id}")]
    public async Task<ActionResult> Post([FromRoute] string id, [FromBody] RetailerInfoRequest data, CancellationToken token)
    {
        var result = await Mediator.Execute(data.Adapt<UpdateRetailerCommand>(), token);
        return HandleCommandResult(result);
    }
}