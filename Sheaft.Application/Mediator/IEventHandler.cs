using Sheaft.Domain.Events;

namespace Sheaft.Application.Mediator
{
    public interface IEventHandler<TDomainEvent> : MediatR.INotificationHandler<DomainEventNotification<TDomainEvent>>
        where TDomainEvent : DomainEvent
    {
    }
    
    public class DomainEventNotification<TDomainEvent> : MediatR.INotification where TDomainEvent : DomainEvent
    {
        public DomainEventNotification(TDomainEvent domainEvent)
        {
            DomainEvent = domainEvent;
        }

        public TDomainEvent DomainEvent { get; }
    }
}