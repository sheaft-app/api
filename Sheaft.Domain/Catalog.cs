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

        public CatalogProduct RemoveProduct(Guid productId)
        {
            if (Products == null)
                throw SheaftException.NotFound();

            var product = Products.SingleOrDefault(p => p.ProductId == productId);
            if (product == null)
                throw SheaftException.NotFound();

            Products.Remove(product);
            return product;
        }

        public void AddOrUpdateProduct(Product product, decimal wholeSalePrice)
        {
            if (Products == null)
                Products = new List<CatalogProduct>();

            var existingProductPrice = Products.SingleOrDefault(p => p.ProductId == product.Id);
            if (existingProductPrice != null)
                existingProductPrice.SetWholeSalePricePerUnit(wholeSalePrice);
            else
                Products.Add(new CatalogProduct(Guid.NewGuid(), product, this, wholeSalePrice));
        }
    }
}