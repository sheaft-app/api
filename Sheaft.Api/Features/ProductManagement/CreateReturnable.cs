using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.ProductManagement;

namespace Sheaft.Api.ProductManagement;

[Route(Routes.RETURNABLES)]
public class CreateReturnable : Feature
{
    public CreateReturnable(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    [HttpPost("")]
    public async Task<ActionResult<string>> Post([FromBody] CreateReturnableRequest data, CancellationToken token)
    {
        var result = await Mediator.Execute(new CreateReturnableCommand(data.Name, data.Code,  data.Price, data.Vat, CurrentSupplierId), token);
        return HandleCommandResult(result);
    }
}

public record CreateReturnableRequest(string Name, string Code, int Price, int Vat);
