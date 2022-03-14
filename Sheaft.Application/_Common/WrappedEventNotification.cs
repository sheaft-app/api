using MediatR;
using Sheaft.Domain;

namespace Sheaft.Application;

public class WrappedEventNotification<T> : INotification where T : IDomainEvent
{
    public WrappedEventNotification(T @event)
    {
        Event = @event;
    }

    public T Event { get; }
}