using Microsoft.AspNetCore.Mvc;
using Sheaft.Application;
using Sheaft.Application.AccountManagement;

namespace Sheaft.Api.AccountManagement;

[Route(Routes.PROFILE)]
public class GetProfile : Feature
{
    public GetProfile(ISheaftMediator mediator)
        : base(mediator)
    {
    }

    [HttpGet("")]
    public async Task<ActionResult<ProfileDto>> Get(CancellationToken token)
    {
        var result = await Mediator.Query(new GetProfileQuery(CurrentUserId), token);
        return HandleQueryResult(result);
    }
}