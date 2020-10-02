using Microsoft.Extensions.Options;
using Sheaft.Options;

namespace Sheaft.Application.Interop
{
    public abstract class EventsHandler
    {
        protected readonly IAppDbContext _context;
        protected readonly ISignalrService _signalrService;
        protected readonly IEmailService _emailService;
        protected readonly EmailTemplateOptions _emailTemplateOptions;

        protected EventsHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService,
            IOptionsSnapshot<EmailTemplateOptions> emailTemplateOptions)
        {
            _context = context;
            _emailService = emailService;
            _signalrService = signalrService;
            _emailTemplateOptions = emailTemplateOptions.Value;
        }
    }
}