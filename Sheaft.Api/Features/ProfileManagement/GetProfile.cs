using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sheaft.Application.ProfileManagement;

namespace Sheaft.Api.ProfileManagement;

[Route(Routes.PROFILE)]
public class GetProfile : Feature
{
    public GetProfile(IMediator mediator)
        : base(mediator)
    {
    }

    [HttpGet("")]
    public async Task<ActionResult<ProfileDto>> Get(CancellationToken token)
    {
        var result = await Mediator.Send(new GetProfileQuery(CurrentUserId), token);
        return HandleQueryResult(result);
    }
}