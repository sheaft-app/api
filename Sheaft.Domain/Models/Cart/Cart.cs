using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Domain.Common;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events;
using Sheaft.Domain.Exceptions;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class Cart : IEntity, IHasDomainEvent
    {
        private const int DIGITS_COUNT = 2;

        protected Cart()
        {
        }

        public Cart(Dictionary<Guid, int> orderProducts, Guid? customerId = null)
        {
            Id = Guid.NewGuid();
            ClientId = customerId;
            Status = CartStatus.Pending;

            Products = new List<CartProduct>();

            SetProducts(orderProducts);
        }

        public Guid Id { get; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset UpdatedOn { get; private set; }
        public bool Removed { get; private set; }
        public CartStatus Status { get; set; }
        public Guid? ClientId { get; private set; }
        public ICollection<CartProduct> Products { get; private set; }
        public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();
        
        public void Restore()
        {
            Removed = false;
        }

        public void AssignToCustomer(Guid customerId)
        {
            if (ClientId.HasValue)
                throw new ConflictException("Un client est déjà lié à ce panier.");

            ClientId = customerId;
        }

        public void SetProducts(Dictionary<Guid, int> productsQuantities)
        {
            if (Products == null || Products.Any())
                Products = new List<CartProduct>();

            foreach (var (key, value) in productsQuantities)
            {
                var product = new CartProduct(key, value);
                if (product.Quantity > 0)
                    Products.Add(product);
            }
        }
    }
}