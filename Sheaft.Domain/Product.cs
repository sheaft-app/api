using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Common;
using Sheaft.Domain.Enum;
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

        public Product(Guid id, string reference, string name, ConditioningKind conditioning, UnitKind unit,
            decimal quantityPerUnit, Producer producer)
        {
            Id = id;
            Producer = producer ?? throw SheaftException.Validation("Le producteur du produit est requis.");
            ProducerId = producer.Id;

            SetName(name);
            SetReference(reference);
            SetConditioning(conditioning, quantityPerUnit, unit);

            Tags = new List<ProductTag>();
            Ratings = new List<Rating>();
            CatalogsPrices = new List<CatalogProduct>();

            DomainEvents = new List<DomainEvent>();
            VisibleTo = VisibleToKind.None;

            RefreshRatings();
            RefreshPrices();
        }

        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public string Reference { get; private set; }
        public string Name { get; private set; }
        public decimal? Weight { get; private set; }
        public string Description { get; private set; }
        public string Picture { get; private set; }
        public UnitKind Unit { get; private set; }
        public decimal QuantityPerUnit { get; private set; }
        public ConditioningKind Conditioning { get; private set; }
        public decimal Vat { get; private set; }
        public bool Available { get; private set; }
        public int RatingsCount { get; private set; }
        public int TagsCount { get; private set; }
        public int PicturesCount { get; private set; }
        public int CatalogsPricesCount { get; private set; }
        public VisibleToKind VisibleTo { get; private set; }
        public decimal? Rating { get; private set; }

        public Guid? ReturnableId { get; private set; }

        public Guid ProducerId { get; private set; }
        public virtual Returnable Returnable { get; private set; }
        public virtual Producer Producer { get; private set; }
        public virtual ICollection<ProductTag> Tags  { get; private set; }
        public virtual ICollection<Rating> Ratings { get; private set; }
        public virtual ICollection<ProductPicture> Pictures { get; private set; }
        public virtual ICollection<CatalogProduct> CatalogsPrices  { get; private set; }

        public void SetReference(string reference)
        {
            if (string.IsNullOrWhiteSpace(reference))
                throw SheaftException.Validation("La référence est requise.");

            Reference = reference;
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw SheaftException.Validation("Le nom est requis.");

            Name = name;
        }

        public void SetAvailable(bool? available)
        {
            Available = available ?? Available;
        }

        public void SetTags(IEnumerable<Tag> tags)
        {
            if (Tags == null)
                Tags = new List<ProductTag>();

            Tags.Clear();

            if (tags?.Any() == true)
                AddTags(tags);
        }
        
        public void SetWeight(decimal? newWeight)
        {
            if (!newWeight.HasValue)
                return;

            if (newWeight <= 0)
                throw SheaftException.Validation("Le poids ne peut être inférieur à 0.");

            Weight = Math.Round(newWeight.Value, DIGITS_COUNT);
        }

        public void SetVat(decimal? newVat)
        {
            if (Producer.NotSubjectToVat)
                newVat = 0;

            if (!newVat.HasValue)
                throw SheaftException.Validation("La TVA est requise.");

            if (newVat < 0)
                throw SheaftException.Validation("La TVA ne peut être inférieure à 0%.");

            if (newVat > 100)
                throw SheaftException.Validation("La TVA ne peut être supérieure à 100%.");

            Vat = newVat.Value;
            RefreshPrices();
        }

        public void SetDescription(string newDescription)
        {
            if (newDescription == null)
                return;

            if (!string.IsNullOrWhiteSpace(newDescription) && newDescription.Length > DESCRIPTION_MAXLENGTH)
                throw SheaftException.Validation($"La description ne peut pas dépasser {DESCRIPTION_MAXLENGTH} caractères.");

            Description = newDescription;
        }

        public void AddRating(User user, decimal value, string comment)
        {
            if (Ratings == null)
                Ratings = new List<Rating>();

            if (Ratings.Any(r => r.UserId == user.Id))
                throw SheaftException.Validation("Vous avez déjà noté ce produit.");

            Ratings.Add(new Rating(Guid.NewGuid(), value, user, comment));
            RefreshRatings();
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
                throw SheaftException.Validation("Le conditionnement est requis.");

            if (conditioning != ConditioningKind.Bulk)
                unit = UnitKind.NotSpecified;

            if (conditioning == ConditioningKind.Bouquet || conditioning == ConditioningKind.Bunch)
                quantity = 1;

            if (quantity <= 0)
                throw SheaftException.Validation("La quantité par conditionnement ne peut pas être inférieure à 0.");

            if (conditioning == ConditioningKind.Bulk && unit == UnitKind.NotSpecified)
                throw SheaftException.Validation("L'unité du type de conditionnement est requis.");

            Conditioning = conditioning;
            QuantityPerUnit = quantity;
            Unit = unit;

            RefreshPrices();
        }

        public void AddPicture(ProductPicture picture)
        {
            if (Pictures == null)
                Pictures = new List<ProductPicture>();

            if(Pictures.Any(p => p.Position == picture.Position))
                foreach (var productPicture in Pictures.Where(p => p.Position >= picture.Position))
                    productPicture.IncreasePosition();
            
            Pictures.Add(picture);
            if (picture.Position == 0)
                Picture = picture.Url;

            PicturesCount = Pictures.Count;
        }

        private void RefreshPrices()
        {
            if (CatalogsPrices == null)
                return;

            foreach (var price in CatalogsPrices)
                price.RefreshPrice(Vat, Unit, QuantityPerUnit);
        }

        private void AddTags(IEnumerable<Tag> tags)
        {
            if (Tags == null)
                Tags = new List<ProductTag>();

            foreach (var tag in tags)
                Tags.Add(new ProductTag(tag));

            TagsCount = Tags?.Count ?? 0;
        }

        private void RefreshRatings()
        {
            Rating = Ratings.Any() ? Ratings.Average(r => r.Value) : (decimal?) null;
            RatingsCount = Ratings.Count;
        }

        public void AddOrUpdateCatalogPrice(Catalog catalog, decimal wholeSalePricePerUnit)
        {
            if (CatalogsPrices == null)
                CatalogsPrices = new List<CatalogProduct>();

            var existingCatalogPrice = CatalogsPrices.SingleOrDefault(c => c.CatalogId == catalog.Id);
            if (existingCatalogPrice == null)
                CatalogsPrices.Add(new CatalogProduct(Guid.NewGuid(), this, catalog, wholeSalePricePerUnit));
            else
                existingCatalogPrice.SetWholeSalePricePerUnit(wholeSalePricePerUnit);

            CatalogsPricesCount = CatalogsPrices?.Count ?? 0;
            UpdateVisibleToOnAddCatalog(catalog);
        }

        public void RemoveFromCatalog(Guid catalogId)
        {
            if (CatalogsPrices == null || !CatalogsPrices.Any())
                throw SheaftException.NotFound("Ce produit appartient à aucun catalogue.");

            var existingCatalogPrice = CatalogsPrices.SingleOrDefault(c => c.CatalogId == catalogId);
            if (existingCatalogPrice == null)
                throw SheaftException.NotFound("Ce produit n'est pas présent dans le catalogue.");

            CatalogsPrices.Remove(existingCatalogPrice);
            CatalogsPricesCount = CatalogsPrices?.Count ?? 0;
            
            existingCatalogPrice.Catalog.DecreaseProductsCount();
            
            UpdateVisibleToOnRemoveCatalog(existingCatalogPrice);
        }

        public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();
        public byte[] RowVersion { get; private set; }
        

        private void UpdateVisibleToOnAddCatalog(Catalog catalog)
        {
            if (catalog.Kind == CatalogKind.Consumers && VisibleTo == VisibleToKind.None)
            {
                VisibleTo = VisibleToKind.Consumers;
            }

            if (catalog.Kind == CatalogKind.Consumers && VisibleTo == VisibleToKind.Stores)
            {
                VisibleTo = VisibleToKind.All;
            }

            if (catalog.Kind == CatalogKind.Stores && VisibleTo == VisibleToKind.None)
            {
                VisibleTo = VisibleToKind.Stores;
            }

            if (catalog.Kind == CatalogKind.Stores && VisibleTo == VisibleToKind.Consumers)
            {
                VisibleTo = VisibleToKind.All;
            }
        }

        private void UpdateVisibleToOnRemoveCatalog(CatalogProduct? existingCatalogPrice)
        {
            if (existingCatalogPrice.Catalog.Kind == CatalogKind.Consumers && VisibleTo == VisibleToKind.All)
            {
                VisibleTo = VisibleToKind.Stores;
            }

            if (existingCatalogPrice.Catalog.Kind == CatalogKind.Consumers && VisibleTo == VisibleToKind.Consumers)
            {
                VisibleTo = VisibleToKind.None;
            }

            if (existingCatalogPrice.Catalog.Kind == CatalogKind.Stores && VisibleTo == VisibleToKind.All &&
                CatalogsPricesCount < 2)
            {
                VisibleTo = VisibleToKind.Consumers;
            }

            if (existingCatalogPrice.Catalog.Kind == CatalogKind.Stores && VisibleTo == VisibleToKind.Stores &&
                CatalogsPricesCount < 1)
            {
                VisibleTo = VisibleToKind.None;
            }
        }

        public void ClearPictures()
        {
            if (Pictures == null || Pictures.Any())
                Pictures = new List<ProductPicture>();
        }
    }
}