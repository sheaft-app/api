using System;
using System.Collections.Generic;
using Sheaft.Domain.Common;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public abstract class Surveillance : IIdEntity, ITrackCreation, ITrackUpdate, ITrackRemove, IHasDomainEvent
    {
        protected Surveillance(){}
        
        protected Surveillance(Guid id, string comment, Producer producer)
        {
            Id = id;
            Comment = comment;
            ProducerId = producer.Id;
            Producer = producer;
        }
        
        public Guid Id { get; private set; }
        public string Comment { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public Guid ProducerId { get; private set; }
        public virtual Producer Producer { get; private set; }
        public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();
        
        public void SetComment(string comment)
        {
            Comment = comment;
        }
    }
}