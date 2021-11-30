using System;
using System.Collections.Generic;
using Sheaft.Domain.Events;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain.BaseClass
{
    public abstract class Surveillance : IEntity, IHasDomainEvent
    {
        protected Surveillance(){}
        
        protected Surveillance(string comment, Company supplier)
        {
            Comment = comment;
            SupplierId = supplier.Id;
            Supplier = supplier;
        }

        public Guid Id { get; } = Guid.NewGuid();
        public string Comment { get; set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset UpdatedOn { get; private set; }
        public bool Removed { get; private set; }
        public Guid SupplierId { get; private set; }
        public Company Supplier { get; private set; }
        public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();
        
        public void Restore()
        {
            Removed = false;
        }
    }
}