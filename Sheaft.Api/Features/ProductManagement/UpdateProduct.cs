using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.ProductManagement;
using Sheaft.Domain;

namespace Sheaft.Api.ProductManagement;

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
        var result = await Mediator.Execute(new UpdateProductCommand(new ProductId(id), data.Name, data.Code, data.Description, data.Price), token);
        return HandleCommandResult(result);
    }
}

public record UpdateProductRequest(string Name, string? Code, int Price, string? Description);
