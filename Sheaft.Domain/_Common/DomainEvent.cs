namespace Sheaft.Domain;

public interface IDomainEvent
{
    Guid EventId { get; }
    DateTimeOffset OccuredAt { get; }
    public bool Published { get; set; }
}

public abstract record DomainEvent : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTimeOffset OccuredAt { get; } = DateTimeOffset.UtcNow;
    public bool Published { get; set; } = false;
}