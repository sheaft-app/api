using Sheaft.Domain.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Domain.Enums;
using Sheaft.Domains.Exceptions;

namespace Sheaft.Domain.Models
{
    public class QuickOrder : IEntity
    {
        private List<QuickOrderProduct> _products;

        protected QuickOrder() { }

        public QuickOrder(Guid id, string name, IDictionary<Product, int> products, User user)
        {
            if (user == null)
                throw new ValidationException(MessageKind.QuickOrder_User_Required);

            Id = id;

            SetName(name);
            SetAsDefault();
            User = user;

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
        public virtual IEnumerable<QuickOrderProduct> Products { get { return _products?.AsReadOnly(); } private set { } }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationException(MessageKind.QuickOrder_Name_Required);

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

        public void AddProducts(IDictionary<Product, int> products)
        {
            foreach (var product in products)
                AddProduct(product);
        }

        public void AddProduct(KeyValuePair<Product, int> product)
        {
            var productLine = _products.SingleOrDefault(p => p.Product.Id == product.Key.Id);
            if (productLine != null)
                throw new ValidationException(MessageKind.QuickOrder_CannotAddProduct_Product_AlreadyIn);
            
            _products.Add(new QuickOrderProduct(product.Key, product.Value));            
        }

        public void RemoveProducts(IEnumerable<Product> products)
        {
            foreach (var product in products)
                RemoveProduct(product);
        }

        public void RemoveProduct(Product product)
        {
            var productLine = _products.SingleOrDefault(p => p.Product.Id == product.Id);
            if (productLine == null)
                throw new ValidationException(MessageKind.QuickOrder_CannotRemoveProduct_Product_NotFound);

            _products.Remove(productLine);
        }
    }
}