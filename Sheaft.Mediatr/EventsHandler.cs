using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;

namespace Sheaft.Mediatr
{
    public abstract class EventsHandler
    {
        protected readonly IAppDbContext _context;
        protected readonly ISignalrService _signalrService;
        protected readonly IEmailService _emailService;

        protected EventsHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
        {
            _context = context;
            _emailService = emailService;
            _signalrService = signalrService;
        }
    }
}