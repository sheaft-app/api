using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Sheaft.Application.Commands;
using Sheaft.Core.Security;
using Sheaft.Core;
using Sheaft.Interop.Enums;
using Sheaft.Models.Inputs;
using Sheaft.Options;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sheaft.Api.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("contact")]
    public class ContactController : ControllerBase
    {
        private readonly LandingOptions _landingOptions;
        private readonly IMediator _mediatr;

        public ContactController(IOptionsSnapshot<LandingOptions> landingOptions, IMediator mediator)
        {
            _landingOptions = landingOptions.Value;
            _mediatr = mediator;
        }

        [HttpPost("newsletter")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> RegisterNewsletter([FromForm] RegisterNewsletterInput model, CancellationToken token)
        {
            var results = await _mediatr.Send(new CreateContactCommand(new RequestUser("contact-user", HttpContext.TraceIdentifier)) { FirstName = model.FirstName, Role = model.Role, Email = model.Email }, token);
            var origin = Request.Headers.FirstOrDefault(c => c.Key == HeaderNames.Origin).Value;

            if (!results.Success)
            {
                if (results.Exception.Kind != ExceptionKind.Unexpected)
                    return new RedirectResult(origin + "?error=" + results.Message.Value);

                return new RedirectResult(origin + "?error=Une%20erreur%20inattendue%20est%20survenue.");
            }

            var url = origin + _landingOptions.NewsletterUrl;
            return new RedirectResult(url);
        }
    }
}