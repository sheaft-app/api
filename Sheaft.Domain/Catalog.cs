using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class Catalog : IEntity
    {
        private List<CatalogProduct> _products;

        protected Catalog()
        {
        }

        public Catalog(Producer producer, Guid id, string name)
        {
            Id = id;
            Name = name;
            Producer = producer;
        }

        public Guid Id { get; }
        public DateTimeOffset CreatedOn { get; }
        public DateTimeOffset? UpdatedOn { get; }
        public DateTimeOffset? RemovedOn { get; }
        public string Name { get; private set;}
        public bool IsDefaultForStores { get; private set; }
        public bool VisibleToStores { get; private set;}
        public bool VisibleToConsumers { get; private set;}
        public virtual Producer Producer { get; private set;}
        public virtual IReadOnlyCollection<CatalogProduct> Products => _products?.AsReadOnly();

        public void SetAsDefaultCatalogForStore()
        {
            if (!VisibleToStores)
                throw SheaftException.Validation();
            
            IsDefaultForStores = true;
        }

        public void SetVisibleToConsumers(bool visibileToConsumers)
        {
            VisibleToConsumers = visibileToConsumers;
        }

        public void SetVisibleToStores(bool visibileToStores)
        {
            if (IsDefaultForStores)
                throw SheaftException.Validation();
            
            VisibleToStores = visibileToStores;
        }
        
        public void AddProduct(Product product, decimal wholeSalePrice)
        {
            if (_products == null)
                _products = new List<CatalogProduct>();
            
            if (_products != null && _products.Any(p => p.Product.Id == product.Id))
                throw SheaftException.AlreadyExists();
            
            _products.Add(new CatalogProduct(product, this, wholeSalePrice));
        }
        
        public void RemoveProduct(Guid productId)
        {
            if (_products == null)
                throw SheaftException.NotFound();

            var product = _products.SingleOrDefault(p => p.Product.Id == productId);
            if(product == null)
                throw SheaftException.NotFound();

            _products.Remove(product);
        }

        public void SetProductWholeSalePricePerUnit(Guid productId, decimal newPrice)
        {
            var productPrice = _products?.SingleOrDefault(cp => cp.Product.Id == productId);
            if (productPrice == null)
                throw SheaftException.NotFound();
            
            productPrice.SetWholeSalePricePerUnit(newPrice);
        }
    }
}