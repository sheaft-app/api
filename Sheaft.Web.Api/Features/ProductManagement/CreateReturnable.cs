using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.ProductManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.ProductManagement;

#pragma warning disable CS8604
[Route(Routes.SUPPLIER_RETURNABLES)]
public class CreateReturnable : Feature
{
    public CreateReturnable(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// Create a returnable to be used with products
    /// </summary>
    [HttpPost("", Name = nameof(CreateReturnable))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> Post(string supplierId, CreateReturnableRequest data, CancellationToken token)
    {
        var result =
            await Mediator.Execute(
                new CreateReturnableCommand(data.Name, data.Code, data.UnitPrice, data.Vat, new SupplierId(supplierId)), token);
        return HandleCommandResult(result);
    }
}

public record CreateReturnableRequest(string Name, string Code, decimal UnitPrice, decimal Vat);