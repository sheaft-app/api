using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sheaft.Application.Events;
using Sheaft.Domain.Models;
using Sheaft.Infrastructure.Interop;
using Sheaft.Services.Interop;

namespace Sheaft.Application.Handlers
{

    public class DeclarationEventsHandler :
        INotificationHandler<DeclarationIncompleteEvent>,
        INotificationHandler<DeclarationRefusedEvent>,
        INotificationHandler<DeclarationValidatedEvent>
    {
        private readonly IAppDbContext _context;
        private readonly IEmailService _emailService;

        public DeclarationEventsHandler(IAppDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public Task Handle(DeclarationIncompleteEvent declarationEvent, CancellationToken token)
        {
            return Task.CompletedTask;
        }

        public Task Handle(DeclarationRefusedEvent declarationEvent, CancellationToken token)
        {
            return Task.CompletedTask;
        }

        public Task Handle(DeclarationValidatedEvent declarationEvent, CancellationToken token)
        {
            return Task.CompletedTask;
        }
    }
}