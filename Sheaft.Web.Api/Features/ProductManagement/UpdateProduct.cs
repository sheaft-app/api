using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.ProductManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.ProductManagement;

[Route(Routes.SUPPLIER_PRODUCTS)]
public class UpdateProduct : Feature
{
    public UpdateProduct(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// Update product with id info
    /// </summary>
    [HttpPut("{productId}", Name = nameof(UpdateProduct))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Put(string supplierId, string productId, UpdateProductRequest data, CancellationToken token)
    {
        var result = await Mediator.Execute(new UpdateProductCommand(new SupplierId(supplierId), new ProductId(productId), data.Name, data.Vat, data.Code, data.Description, data.UnitPrice, data.ReturnableId != null ? new ReturnableId(data.ReturnableId) : null), token);
        return HandleCommandResult(result);
    }
}

public record UpdateProductRequest(string Name, string? Code, decimal UnitPrice, decimal Vat, string? Description, string? ReturnableId);
