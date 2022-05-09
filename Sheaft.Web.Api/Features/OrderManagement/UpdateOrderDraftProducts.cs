using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.OrderManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.OrderManagement;

[Route(Routes.ORDERS)]
public class UpdateOrderDraftProducts : Feature
{
    public UpdateOrderDraftProducts(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    [HttpPut("{id}/draft")]
    public async Task<ActionResult> Post([FromRoute] string id, [FromBody] UpdateDraftProductsRequest data, CancellationToken token)
    {
        var result = await Mediator.Execute(new UpdateOrderDraftProductsCommand(new OrderId(id), data.Products), token);
        return HandleCommandResult(result);
    }
}

public record UpdateDraftProductsRequest(IEnumerable<ProductQuantityDto> Products);
