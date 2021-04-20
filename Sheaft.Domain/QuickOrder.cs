using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Common;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class QuickOrder : IEntity, IHasDomainEvent
    {
        private List<QuickOrderProduct> _products;

        protected QuickOrder() { }

        public QuickOrder(Guid id, string name, IDictionary<CatalogProduct, int> products, User user)
        {
            if (user == null)
                throw new ValidationException(MessageKind.QuickOrder_User_Required);

            Id = id;

            SetName(name);
            SetAsDefault();
            User = user;

            DomainEvents = new List<DomainEvent>();
            _products = new List<QuickOrderProduct>();
            AddProducts(products);
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool IsDefault { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public virtual User User { get; private set; }
        public virtual IEnumerable<QuickOrderProduct> Products => _products?.AsReadOnly();

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw SheaftException.Validation(MessageKind.QuickOrder_Name_Required);

            Name = name;
        }

        public void SetDescription(string description)
        {
            if (description == null)
                return;

            Description = description;
        }

        public void SetAsDefault()
        {
            IsDefault = true;
        }

        public void UnsetAsDefault()
        {
            IsDefault = false;
        }

        public void AddProducts(IDictionary<CatalogProduct, int> products)
        {
            if (products == null)
                return;
            
            foreach (var product in products)
                AddOrUpdateProduct(product.Key, product.Value);
        }

        public void AddOrUpdateProduct(CatalogProduct catalogProduct, int quantity)
        {
            if (Products == null)
                _products = new List<QuickOrderProduct>();
            
            var productLine = _products.SingleOrDefault(p => p.CatalogProduct.Product.Id == catalogProduct.Product.Id);
            if (productLine != null)
            {
                productLine.SetQuantity(quantity);
                return;
            }

            _products.Add(new QuickOrderProduct(catalogProduct, quantity)); 
        }

        public void RemoveProduct(QuickOrderProduct product)
        {
            if (Products == null)
                throw SheaftException.NotFound();

            _products.Remove(product);
        }

        public void RemoveProduct(Guid productId)
        {
            if (Products == null)
                throw SheaftException.NotFound();
            
            var productLine = _products.SingleOrDefault(p => p.CatalogProduct.Product.Id == productId);
            if (productLine == null)
                throw SheaftException.Validation(MessageKind.QuickOrder_CannotRemoveProduct_Product_NotFound);

            _products.Remove(productLine);
        }

        public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();
    }
}