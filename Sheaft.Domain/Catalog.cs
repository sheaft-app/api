using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class Catalog : IEntity
    {
        protected Catalog()
        {
        }

        public Catalog(Producer producer, CatalogKind kind, Guid id, string name)
        {
            Id = id;
            Name = name;
            Kind = kind;
            Producer = producer;
            ProducerId = producer.Id;
            Products = new List<CatalogProduct>();
        }

        public Guid Id { get; }
        public DateTimeOffset CreatedOn { get; }
        public DateTimeOffset? UpdatedOn { get; }
        public DateTimeOffset? RemovedOn { get; }
        public string Name { get; private set; }
        public CatalogKind Kind { get; private set; }
        public bool Available { get; private set; }
        public bool IsDefault { get; private set; }
        public Guid ProducerId { get; private set; }
        public virtual Producer Producer { get; private set; }
        public int ProductsCount { get; private set; }
        public virtual ICollection<CatalogProduct> Products { get; private set; }
        public byte[] RowVersion { get; private set; }

        public void SetIsAvailable(bool isAvailable)
        {
            Available = isAvailable;
        }

        public void SetIsDefault(bool isDefault)
        {
            IsDefault = isDefault;
        }

        public void SetName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw SheaftException.Validation("Le nom du catalogue est requis.");
            
            Name = name;
        }

        public void IncreaseProductsCount()
        {
            ProductsCount++;
        }

        public void DecreaseProductsCount()
        {
            ProductsCount--;
        }
    }
}