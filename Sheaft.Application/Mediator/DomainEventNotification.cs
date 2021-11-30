using Sheaft.Domain.Events;

namespace Sheaft.Application
{
    public class DomainEventNotification<TDomainEvent> : MediatR.INotification where TDomainEvent : DomainEvent
    {
        public DomainEventNotification(TDomainEvent domainEvent)
        {
            DomainEvent = domainEvent;
        }

        public TDomainEvent DomainEvent { get; }
    }
}
