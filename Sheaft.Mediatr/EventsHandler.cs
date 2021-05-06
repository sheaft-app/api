using Sheaft.Application.Interfaces;
using Sheaft.Application.Interfaces.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Infrastructure;

namespace Sheaft.Mediatr
{
    public abstract class EventsHandler
    {
        protected readonly ISignalrService _signalrService;
        protected readonly IEmailService _emailService;
        protected IAppDbContext _context;

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