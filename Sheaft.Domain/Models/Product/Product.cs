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
    public class Product : IEntity, IHasDomainEvent
    {
        private const int DESCRIPTION_MAXLENGTH = 500;
        private const int DIGITS_COUNT = 2;

        public Product()
        {
        }

        public Product(string reference, string name, ConditioningKind conditioning, UnitKind unit,
            decimal quantityPerUnit, Company supplier)
        {
            Name = name;
            Reference = reference;
            SupplierId = supplier.Id;
            VisibleTo = VisibleToKind.None;
            SetConditioning(conditioning, quantityPerUnit, unit);

            Tags = new List<ProductTag>();
            Ratings = new List<Rating>();

            DomainEvents = new List<DomainEvent>();
        }

        public Guid Id { get; } = Guid.NewGuid();
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset UpdatedOn { get; private set; }
        public bool Removed { get; private set; }
        public string Reference { get; private set; }
        public string Name { get; private set; }
        public decimal Weight { get; private set; }
        public string Description { get; private set; }
        public string Picture { get; private set; }
        public UnitKind Unit { get; private set; }
        public decimal QuantityPerUnit { get; private set; }
        public ConditioningKind Conditioning { get; private set; }
        public decimal Vat { get; private set; }
        public bool IsEnabled { get; private set; }
        public VisibleToKind VisibleTo { get; private set; }
        public Guid? ReturnableId { get; private set; }
        public Guid SupplierId { get; private set; }
        public Returnable Returnable { get; private set; }
        public ICollection<ProductTag> Tags  { get; private set; }
        public ICollection<Rating> Ratings { get; private set; }
        public ICollection<ProductPicture> Pictures { get; private set; }
        public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();
        
        public void Restore()
        {
            Removed = false;
        }

        public void SetTags(IEnumerable<Tag> tags)
        {
            if (Tags == null)
                Tags = new List<ProductTag>();

            Tags.Clear();

            if (tags?.Any() == true)
                AddTags(tags);
        }

        public void SetVat(decimal? newVat)
        {
            if (!newVat.HasValue)
                throw new ValidationException("La TVA est requise.");

            if (newVat < 0)
                throw new ValidationException("La TVA ne peut être inférieure à 0%.");

            if (newVat > 100)
                throw new ValidationException("La TVA ne peut être supérieure à 100%.");

            Vat = newVat.Value;
        }

        public void SetDescription(string newDescription)
        {
            if (newDescription == null)
                return;

            if (!string.IsNullOrWhiteSpace(newDescription) && newDescription.Length > DESCRIPTION_MAXLENGTH)
                throw new ValidationException($"La description ne peut pas dépasser {DESCRIPTION_MAXLENGTH} caractères.");

            Description = newDescription;
        }

        public void AddRating(User user, decimal value, string comment)
        {
            if (Ratings == null)
                Ratings = new List<Rating>();

            if (Ratings.Any(r => r.UserId == user.Id))
                throw new ValidationException("Vous avez déjà noté ce produit.");

            Ratings.Add(new Rating(value, user, comment));
        }

        public void SetReturnable(Returnable returnable)
        {
            Returnable = returnable;
            ReturnableId = returnable?.Id;
        }

        public void SetConditioning(ConditioningKind conditioning, decimal quantity,
            UnitKind unit = UnitKind.NotSpecified)
        {
            if (conditioning == ConditioningKind.Not_Specified)
                throw new ValidationException("Le conditionnement est requis.");

            if (conditioning != ConditioningKind.Bulk)
                unit = UnitKind.NotSpecified;

            if (conditioning == ConditioningKind.Bouquet || conditioning == ConditioningKind.Bunch)
                quantity = 1;

            if (quantity <= 0)
                throw new ValidationException("La quantité par conditionnement ne peut pas être inférieure à 0.");

            if (conditioning == ConditioningKind.Bulk && unit == UnitKind.NotSpecified)
                throw new ValidationException("L'unité du type de conditionnement est requis.");

            Conditioning = conditioning;
            QuantityPerUnit = quantity;
            Unit = unit;
        }

        public void AddPicture(ProductPicture picture)
        {
            if (Pictures == null)
                Pictures = new List<ProductPicture>();
            
            Pictures.Add(picture);
            if (picture.Position == 0)
                Picture = picture.Url;
        }

        private void AddTags(IEnumerable<Tag> tags)
        {
            if (Tags == null)
                Tags = new List<ProductTag>();

            foreach (var tag in tags)
                Tags.Add(new ProductTag(tag));
        }

        public void UpdateVisibleToOnAddCatalog(CatalogKind catalogKind)
        {
            if (catalogKind == CatalogKind.Consumers && VisibleTo == VisibleToKind.None)
            {
                VisibleTo = VisibleToKind.Consumers;
            }

            if (catalogKind == CatalogKind.Consumers && VisibleTo == VisibleToKind.Stores)
            {
                VisibleTo = VisibleToKind.All;
            }

            if (catalogKind == CatalogKind.Stores && VisibleTo == VisibleToKind.None)
            {
                VisibleTo = VisibleToKind.Stores;
            }

            if (catalogKind == CatalogKind.Stores && VisibleTo == VisibleToKind.Consumers)
            {
                VisibleTo = VisibleToKind.All;
            }
        }

        public void UpdateVisibleToOnRemoveCatalog(CatalogKind catalogKind)
        {
            if (catalogKind == CatalogKind.Consumers && VisibleTo == VisibleToKind.All)
            {
                VisibleTo = VisibleToKind.Stores;
            }

            if (catalogKind == CatalogKind.Consumers && VisibleTo == VisibleToKind.Consumers)
            {
                VisibleTo = VisibleToKind.None;
            }

            if (catalogKind == CatalogKind.Stores && VisibleTo == VisibleToKind.Stores)
            {
                VisibleTo = VisibleToKind.None;
            }

            if (catalogKind == CatalogKind.Stores && VisibleTo == VisibleToKind.All)
            {
                VisibleTo = VisibleToKind.Consumers;
            }
        }

        public void ClearPictures()
        {
            if (Pictures == null || Pictures.Any())
                Pictures = new List<ProductPicture>();
        }
    }
}