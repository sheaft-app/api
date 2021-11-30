using System;
using System.Collections.Generic;
using Sheaft.Domain.Common;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class Catalog : IEntity, IHasDomainEvent
    {
        protected Catalog()
        {
        }

        public Catalog(Guid companyId, CatalogKind kind, Guid id, string name)
        {
            Id = id;
            Name = name;
            Kind = kind;
            SupplierId = companyId;
            Products = new List<CatalogProductPrice>();
        }

        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; }
        public DateTimeOffset UpdatedOn { get; }
        public bool Removed { get; private set; }
        public string Name { get; private set; }
        public CatalogKind Kind { get; private set; }
        public bool IsEnabled { get; set; }
        public bool IsDefault { get; set; }
        public Guid SupplierId { get; }
        public ICollection<CatalogProductPrice> Products { get; private set; }
        public List<DomainEvent> DomainEvents { get; private set; } = new List<DomainEvent>();
        
        public void Restore()
        {
            Removed = false;
        }
    }
}