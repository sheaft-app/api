using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.ProductManagement;
using Sheaft.Domain;

namespace Sheaft.Api.ProductManagement;

[Route(Routes.RETURNABLES)]
public class UpdateReturnable : Feature
{
    public UpdateReturnable(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<string>> Post([FromRoute] string id, [FromBody] UpdateReturnableRequest data, CancellationToken token)
    {
        var result = await Mediator.Execute(new UpdateReturnableCommand(new ReturnableId(id), data.Name, data.Code,  data.Price, data.Vat), token);
        return HandleCommandResult(result);
    }
}

public record UpdateReturnableRequest(string Name, string Code, int Price, int Vat);
