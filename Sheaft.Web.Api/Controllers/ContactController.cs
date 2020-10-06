using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Sheaft.Application.Commands;
using Sheaft.Core;
using Sheaft.Application.Models;
using Sheaft.Options;
using System.Linq;
using Sheaft.Application.Interop;

namespace Sheaft.Web.Api.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("contact")]
    public class ContactController : ControllerBase
    {
        private readonly LandingOptions _landingOptions;
        private readonly ISheaftMediatr _mediatr;

        public ContactController(IOptionsSnapshot<LandingOptions> landingOptions, ISheaftMediatr mediator)
        {
            _landingOptions = landingOptions.Value;
            _mediatr = mediator;
        }

        [HttpPost("newsletter")]
        [Consumes("application/x-www-form-urlencoded")]
        public IActionResult RegisterNewsletter([FromForm] RegisterNewsletterInput model)
        {
            var origin = Request.Headers.FirstOrDefault(c => c.Key == HeaderNames.Origin).Value;

            if (string.IsNullOrWhiteSpace(model.Email))
                return new RedirectResult(origin + "?error=L'adresse mail est requise");

            if (string.IsNullOrWhiteSpace(model.FirstName))
                return new RedirectResult(origin + "?error=Votre prénom est requis");

            if (string.IsNullOrWhiteSpace(model.Role))
                return new RedirectResult(origin + "?error=Vous devez indiquer à quel type de profil vous correspondez.");

            _mediatr.Post(new CreateContactCommand(new RequestUser("contact-user", HttpContext.TraceIdentifier)) { FirstName = model.FirstName, Role = model.Role, Email = model.Email });            
            var url = origin + _landingOptions.NewsletterUrl;
            return new RedirectResult(url);
        }
    }
}