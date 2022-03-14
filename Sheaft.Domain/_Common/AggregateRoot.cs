namespace Sheaft.Domain
{
    public interface IAggregateRoot : IEntity
    {
        IDictionary<int, IDomainEvent> Events { get; }
    }

    public abstract class AggregateRoot : IAggregateRoot
    {
        private object _lock = new { };
        private Dictionary<int, IDomainEvent> _domainEvents = new Dictionary<int, IDomainEvent>();
        
        public IDictionary<int, IDomainEvent> Events => _domainEvents;

        protected void RaiseEvent(IDomainEvent @event)
        {
            lock(_lock)
                _domainEvents.Add(_domainEvents.Count, @event);
        }
    }
}