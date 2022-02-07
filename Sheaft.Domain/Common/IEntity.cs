using System;
using System.Collections.Generic;

namespace Sheaft.Domain.Common;

public interface IEntity
{
    EntityId Identifier{ get; }
}

public interface IAggregateRoot : IEntity
{
    IReadOnlyCollection<IEvent> DomainEvents { get; }
}

public abstract class Entity : IEntity
{
    public EntityId Identifier { get; protected set; } = new();
}

public record EntityId
{
    public EntityId()
    {
        Value = Guid.NewGuid().ToString("N");
    }

    public string Value { get; set; }
}

public abstract class AggregateRoot : Entity, IAggregateRoot
{
    private List<IEvent> _domainEvents = new List<IEvent>();
    
    public IReadOnlyCollection<IEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void RaiseEvent(IEvent @event)
    {
        if(!_domainEvents.Exists(e => e.EventId == @event.EventId))
            _domainEvents.Add(@event);
    }
}