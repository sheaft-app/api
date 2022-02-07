using Sheaft.Domain.Common;

namespace Sheaft.Application.Mediator
{
    public class WrappedEventNotification<T> : MediatR.INotification where T : IIntegrationEvent
    {
        public WrappedEventNotification(T @event)
        {
            Event = @event;
        }

        public T Event { get; }
    }
}