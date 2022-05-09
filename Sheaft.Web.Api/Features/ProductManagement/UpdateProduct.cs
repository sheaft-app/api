using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.ProductManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.ProductManagement;

[Route(Routes.PRODUCTS)]
public class UpdateProduct : Feature
{
    public UpdateProduct(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Post([FromRoute] string id, [FromBody] UpdateProductRequest data, CancellationToken token)
    {
        var result = await Mediator.Execute(new UpdateProductCommand(new ProductId(id), data.Name, data.Vat, data.Code, data.Description, data.Price, data.ReturnableIdentifier != null ? new ReturnableId(data.ReturnableIdentifier) : null), token);
        return HandleCommandResult(result);
    }
}

public record UpdateProductRequest(string Name, string? Code, int Price, int Vat, string? Description, string? ReturnableIdentifier);
