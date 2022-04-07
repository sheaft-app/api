using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.ProductManagement;
using Sheaft.Domain;

namespace Sheaft.Api.ProductManagement;

[Route(Routes.PRODUCTS)]
public class CreateProduct : Feature
{
    public CreateProduct(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    [HttpPost("")]
    public async Task<ActionResult<string>> Post([FromBody] CreateProductRequest data, CancellationToken token)
    {
        var result = await Mediator.Execute(new CreateProductCommand(data.Name, data.Code, data.Description, data.Price, data.Vat, data.ReturnableIdentifier != null ? new ReturnableId(data.ReturnableIdentifier) : null, CurrentSupplierId), token);
        return HandleCommandResult(result);
    }
}

public record CreateProductRequest(string Name, string? Code, int Price, int Vat, string? Description, string? ReturnableIdentifier);
