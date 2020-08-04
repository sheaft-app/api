using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Sheaft.Application.Commands;
using Sheaft.Core.Security;
using Sheaft.Interop.Enums;
using Sheaft.Logging;
using Sheaft.Options;

namespace Sheaft.Functions
{
    public class ContactFunctions
    {
        private readonly LandingOptions _landingOptions;
        private readonly IMediator _mediatr;

        public ContactFunctions(IOptionsSnapshot<LandingOptions> landingOptions, IMediator mediator)
        {
            _landingOptions = landingOptions.Value;
            _mediatr = mediator;
        }

        [FunctionName("CreateContactCommand")]
        public async Task<IActionResult> CreateContactCommandAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req, ILogger logger, CancellationToken token)
        {
            var results = await _mediatr.Send(new CreateContactCommand(new RequestUser("contact-functions", req.HttpContext.TraceIdentifier)) { FirstName = req.Form["FirstName"], Role = req.Form["Role"], Email = req.Form["Email"] }, token);
            var origin = req.Headers.FirstOrDefault(c => c.Key == HeaderNames.Origin).Value;
            logger.LogCommand(results);

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
