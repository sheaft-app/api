using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Domain.Common;
using Sheaft.Domain.Events;
using Sheaft.Domain.Exceptions;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class QuickOrder : IEntity, IHasDomainEvent
    {
        protected QuickOrder() { }

        public QuickOrder(string name, IDictionary<CatalogProductPrice, int?> products, User user)
        {
            Name = name;
            User = user;
            UserId = user.Id;
            CompanyId = user.CompanyId.Value;

            DomainEvents = new List<DomainEvent>();
            Products = new List<QuickOrderProduct>();
            
            AddProducts(products);
        }

        public Guid Id { get; } = Guid.NewGuid();
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool IsDefault { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset UpdatedOn { get; private set; }
        public bool Removed { get; private set; }
        public Guid UserId { get; private set; }
        public Guid CompanyId { get; private set; }
        public User User { get; private set; }
        public ICollection<QuickOrderProduct> Products { get; private set; }
        public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();
        
        public void Restore()
        {
            Removed = false;
        }

        public void AddProducts(IDictionary<CatalogProductPrice, int?> products)
        {
            if (products == null)
                return;
            
            foreach (var product in products)
                AddOrUpdateProduct(product.Key, product.Value);
        }

        public void AddOrUpdateProduct(CatalogProductPrice catalogProductPrice, int? quantity)
        {
            if (Products == null)
                Products = new List<QuickOrderProduct>();
            
            var productLine = Products.SingleOrDefault(p => p.ProductId == catalogProductPrice.ProductId);
            if (productLine != null)
                Products.Remove(productLine);

            Products.Add(new QuickOrderProduct(catalogProductPrice, quantity));
        }

        public void RemoveProduct(Guid productId)
        {
            if (Products == null)
                throw new NotFoundException("Ce modèle de commande ne contient aucun produits.");
            
            var productLine = Products.SingleOrDefault(p => p.ProductId == productId);
            if (productLine == null)
                throw new ValidationException("Impossible de supprimer le produit du modèle de commande, il est introuvable.");

            Products.Remove(productLine);
        }
    }
}