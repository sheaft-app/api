using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.ProductManagement;
using Sheaft.Domain;

namespace Sheaft.Api.ProductManagement;

[Route(Routes.PRODUCTS)]
public class DeleteProduct : Feature
{
    public DeleteProduct(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Post([FromRoute] string id, CancellationToken token)
    {
        var result = await Mediator.Execute(new RemoveProductCommand(new ProductId(id)), token);
        return HandleCommandResult(result);
    }
}
