using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.ProductManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.ProductManagement;

[Route(Routes.PRODUCTS)]
public class DeleteProduct : Feature
{
    public DeleteProduct(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// Remove product with id
    /// </summary>
    [HttpDelete("{id}", Name = nameof(DeleteProduct))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Post([FromRoute] string id, CancellationToken token)
    {
        var result = await Mediator.Execute(new RemoveProductCommand(new ProductId(id)), token);
        return HandleCommandResult(result);
    }
}
