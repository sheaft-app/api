﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sheaft.Application.Interfaces.Services;
using Sheaft.Domain;
using Sheaft.Domain.Events.Producer;

namespace Sheaft.Mediatr.Producer.EventHandlers
{
    public class ProducerDocumentsNotValidatedEventHandler : EventsHandler,
        INotificationHandler<DomainEventNotification<ProducerDocumentsNotValidatedEvent>>
    {
        public ProducerDocumentsNotValidatedEventHandler(
            IAppDbContext context,
            IEmailService emailService,
            ISignalrService signalrService)
            : base(context, emailService, signalrService)
        {
        }
        
        public async Task Handle(DomainEventNotification<ProducerDocumentsNotValidatedEvent> notification, CancellationToken token)
        {
            var producerEvent = notification.DomainEvent;
            var legal = await _context.Set<BusinessLegal>().SingleOrDefaultAsync(c => c.UserId == producerEvent.ProducerId, token);
            await _emailService.SendEmailAsync(
               "support@sheaft.com",
               "Support",
               $"La validation de la configuration des documents du producteur {legal.User.Name} a échouée",
               $"La validation de la configuration des documents du producteur {legal.User.Name} ({legal.User.Email}) a échouée. Veuillez vérifier la configuration des documents depuis le site https://manage.sheaft.com.",
               false,
               token);
        }
    }
}