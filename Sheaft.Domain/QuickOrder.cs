using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Common;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class QuickOrder : IEntity, IHasDomainEvent
    {
        protected QuickOrder() { }

        public QuickOrder(Guid id, string name, IDictionary<CatalogProduct, int?> products, User user)
        {
            if (user == null)
                SheaftException.Validation("Impossible de créer le modèle de commande, l'utilisateur est requis.");

            Id = id;

            SetName(name);
            User = user;
            UserId = user.Id;

            DomainEvents = new List<DomainEvent>();
            Products = new List<QuickOrderProduct>();
            AddProducts(products);
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool IsDefault { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public Guid UserId { get; private set; }
        public int ProductsCount { get; private set; }
        public virtual User User { get; private set; }
        public virtual ICollection<QuickOrderProduct> Products { get; private set; }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw SheaftException.Validation("Le nom est requis.");

            Name = name;
        }

        public void SetDescription(string description)
        {
            if (description == null)
                return;

            Description = description;
        }

        public void SetIsDefault(bool isDefault)
        {
            IsDefault = isDefault;
        }

        public void AddProducts(IDictionary<CatalogProduct, int?> products)
        {
            if (products == null)
                return;
            
            foreach (var product in products)
                AddOrUpdateProduct(product.Key, product.Value);
        }

        public void AddOrUpdateProduct(CatalogProduct catalogProduct, int? quantity)
        {
            if (Products == null)
                Products = new List<QuickOrderProduct>();
            
            var productLine = Products.SingleOrDefault(p => p.CatalogProduct.ProductId == catalogProduct.ProductId);
            if (productLine != null)
            {
                productLine.SetQuantity(quantity);
                return;
            }

            Products.Add(new QuickOrderProduct(catalogProduct, quantity));
            ProductsCount = Products?.Count ?? 0;
        }

        public QuickOrderProduct RemoveProduct(Guid productId)
        {
            if (Products == null)
                throw SheaftException.NotFound("Ce modèle de commande ne contient aucun produits.");
            
            var productLine = Products.SingleOrDefault(p => p.CatalogProduct.ProductId == productId);
            if (productLine == null)
                throw SheaftException.Validation("Impossible de supprimer le produit du modèle de commande, il est introuvable.");

            Products.Remove(productLine);
            ProductsCount = Products?.Count ?? 0;

            return productLine;
        }

        public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();
        public byte[] RowVersion { get; private set; }
    }
}