using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.ProductManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.ProductManagement;

[Route(Routes.SUPPLIER_PRODUCTS)]
public class DeleteProduct : Feature
{
    public DeleteProduct(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// Remove product with id
    /// </summary>
    [HttpDelete("{productId}", Name = nameof(DeleteProduct))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Post(string supplierId, string productId, CancellationToken token)
    {
        var result = await Mediator.Execute(new RemoveProductCommand(new ProductId(productId), new SupplierId(supplierId)), token);
        return HandleCommandResult(result);
    }
}
