using System;

namespace Sheaft.Domain.Common
{
    public interface IEvent
    {
        EventId EventId { get; }
        DateTimeOffset OccuredAt { get; }
    }

    public record EventId
    {
        public EventId()
        {
            Value = Guid.NewGuid();
        }

        public Guid Value { get; }
    }

    public interface IDomainEvent : IEvent
    {
    }

    public interface IIntegrationEvent : IEvent
    {
    }

    public abstract record Event : IEvent
    {
        public EventId EventId { get; } = new();
        public DateTimeOffset OccuredAt { get; } = DateTimeOffset.UtcNow;
        public bool Processed { get; set; }
    }
}