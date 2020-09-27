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
using Sheaft.Application.Interop;
using Sheaft.Core;
using Sheaft.Core.Extensions;
using Sheaft.Exceptions;
using Sheaft.Options;

namespace Sheaft.Functions
{
    public class ContactFunctions
    {
        private readonly LandingOptions _landingOptions;
        private readonly ISheaftMediatr _mediatr;

        public ContactFunctions(IOptionsSnapshot<LandingOptions> landingOptions, ISheaftMediatr mediator)
        {
            _landingOptions = landingOptions.Value;
            _mediatr = mediator;
        }

        [FunctionName("CreateContactCommand")]
        public async Task<IActionResult> CreateContactCommandAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest request, CancellationToken token)
        {
            var origin = request.Headers.FirstOrDefault(c => c.Key == HeaderNames.Origin).Value;
            var results = await _mediatr.Process(
                new CreateContactCommand(new RequestUser("contact-functions", request.HttpContext.TraceIdentifier)) {
                    FirstName = request.Form["FirstName"],
                    Role = request.Form["Role"],
                    Email = request.Form["Email"]
                }, token);
            
            if (!results.Success)
            {
                if (results.Exception.Kind != ExceptionKind.Unexpected)
                    return new RedirectResult(origin + "?error=" + results.Message);

                return new RedirectResult(origin + "?error=Une%20erreur%20inattendue%20est%20survenue.");
            }

            var url = origin + _landingOptions.NewsletterUrl;
            return new RedirectResult(url);
        }
    }
}
