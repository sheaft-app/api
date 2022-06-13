using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.AgreementManagement;
using Sheaft.Domain;

namespace Sheaft.Web.Api.AgreementManagement;

#pragma warning disable CS8604
[Route(Routes.AGREEMENTS)]
public class GetAvailableCustomer : Feature
{
    public GetAvailableCustomer(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    /// <summary>
    /// Retrieve customer with id
    /// </summary>
    [HttpGet("customers/{id}", Name = nameof(GetAvailableCustomer))]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AvailableCustomerDto>> Get(string id, CancellationToken token)
    {
        var result = await Mediator.Query(new GetAvailableCustomerQuery(new CustomerId(id)), token);
        return HandleCommandResult(result);
    }
}