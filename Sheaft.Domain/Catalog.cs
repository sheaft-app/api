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
        private List<CatalogProduct> _products;

        protected Catalog()
        {
        }
        
        public Catalog(Producer producer, CatalogKind kind, Guid id, string name)
        {
            Id = id;
            Name = name;
            Kind = kind;
            Producer = producer;
        }

        public Guid Id { get; }
        public DateTimeOffset CreatedOn { get; }
        public DateTimeOffset? UpdatedOn { get; }
        public DateTimeOffset? RemovedOn { get; }
        public string Name { get; private set; }
        public CatalogKind Kind { get; private set; }
        public bool Available { get; private set; }
        public bool IsDefault { get; private set; }
        public virtual Producer Producer { get; private set; }
        public virtual IReadOnlyCollection<CatalogProduct> Products => _products?.AsReadOnly();

        public void SetIsAvailable(bool isAvailable)
        {
            Available = isAvailable;
        }

        public void SetIsDefault(bool isDefault)
        {
            IsDefault = isDefault;
        }

        public void RemoveProduct(Guid productId)
        {
            if (_products == null)
                throw SheaftException.NotFound();

            var product = _products.SingleOrDefault(p => p.Product.Id == productId);
            if (product == null)
                throw SheaftException.NotFound();

            _products.Remove(product);
        }

        public void AddOrUpdateProduct(Product product, decimal wholeSalePrice)
        {
            if (_products == null)
                _products = new List<CatalogProduct>();

            var existingProductPrice = _products.SingleOrDefault(p => p.Product.Id == product.Id);
            if (existingProductPrice != null)
                existingProductPrice.SetWholeSalePricePerUnit(wholeSalePrice);
            else
                _products.Add(new CatalogProduct(product, this, wholeSalePrice));
        }
    }
}