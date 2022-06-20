using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.ProductManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.ProductManagement;

#pragma warning disable CS8604
[Route(Routes.SUPPLIER_PRODUCTS)]
public class CreateProduct : Feature
{
    public CreateProduct(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// Create a product
    /// </summary>
    [HttpPost("", Name = nameof(CreateProduct))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> Post(string supplierId, CreateProductRequest data, CancellationToken token)
    {
        var result =
            await Mediator.Execute(
                new CreateProductCommand(data.Name, data.Code, data.Description, data.UnitPrice, data.Vat,
                    data.ReturnableId != null ? new ReturnableId(data.ReturnableId) : null,
                    new SupplierId(supplierId)), token);
        return HandleCommandResult(result);
    }
}

public record CreateProductRequest(string Name, string? Code, decimal UnitPrice, decimal Vat, string? Description,
    string? ReturnableId);