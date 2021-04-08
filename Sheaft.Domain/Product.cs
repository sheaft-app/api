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

        private List<ProductTag> _tags;
        private List<Rating> _ratings;
        private List<ProductPicture> _pictures;
        private List<CatalogProduct> _catalogsPrices;

        protected Product()
        {
        }

        public Product(Guid id, string reference, string name, ConditioningKind conditioning, UnitKind unit, decimal quantityPerUnit, Producer producer)
        {
            Id = id;
            Producer = producer ?? throw new ValidationException(MessageKind.Product_Producer_Required);

            SetName(name);
            SetReference(reference);
            SetConditioning(conditioning, quantityPerUnit, unit);

            _tags = new List<ProductTag>();
            _ratings = new List<Rating>();
            _catalogsPrices = new List<CatalogProduct>();
            
            DomainEvents = new List<DomainEvent>();

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
        public int RatingsCount { get; set; }
        public decimal? Rating { get; set; }
        public virtual Returnable Returnable { get; private set; }
        public virtual Producer Producer { get; private set; }
        public virtual IReadOnlyCollection<ProductTag> Tags => _tags?.AsReadOnly();
        public virtual IReadOnlyCollection<Rating> Ratings => _ratings?.AsReadOnly();
        public virtual IReadOnlyCollection<ProductPicture> Pictures => _pictures?.AsReadOnly();
        public virtual IReadOnlyCollection<CatalogProduct> CatalogsPrices => _catalogsPrices?.AsReadOnly();

        public void SetReference(string reference)
        {
            if (string.IsNullOrWhiteSpace(reference))
                throw new ValidationException(MessageKind.Product_Reference_Required);

            Reference = reference;
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationException(MessageKind.Product_Name_Required);

            Name = name;
        }

        public void SetAvailable(bool? available)
        {
            Available = available ?? Available;
        }

        public void SetTags(IEnumerable<Tag> tags)
        {
            if(Tags == null)
                _tags = new List<ProductTag>();

            _tags.Clear();

            if (tags?.Any() == true)
                AddTags(tags);
        }

        public void SetPicture(string picture)
        {
            if (picture == null)
                return;

            Picture = picture;
        }

        public void SetWeight(decimal? newWeight)
        {
            if (!newWeight.HasValue)
                return;

            if (newWeight <= 0)
                throw new ValidationException(MessageKind.Product_Weight_CannotBe_LowerOrEqualThan, 0);

            Weight = Math.Round(newWeight.Value, DIGITS_COUNT);
        }

        public void SetVat(decimal? newVat)
        {
            if (Producer.NotSubjectToVat)
                newVat = 0;
            
            if(!newVat.HasValue)
                throw new ValidationException(MessageKind.Product_Vat_Required);

            if (newVat < 0)
                throw new ValidationException(MessageKind.Product_Vat_CannotBe_LowerThan, 0);

            if (newVat > 100)
                throw new ValidationException(MessageKind.Product_Vat_CannotBe_GreaterThan, 100);

            Vat = newVat.Value;
            RefreshPrices();
        }

        public void SetDescription(string newDescription)
        {
            if (newDescription == null)
                return;

            if (!string.IsNullOrWhiteSpace(newDescription) && newDescription.Length > DESCRIPTION_MAXLENGTH)
                throw new ValidationException(MessageKind.Product_Description_TooLong, DESCRIPTION_MAXLENGTH);

            Description = newDescription;
        }

        public void AddRating(User user, decimal value, string comment)
        {
            if (Ratings == null)
                _ratings = new List<Rating>();

            if (_ratings.Any(r => r.User.Id == user.Id))
                throw new ValidationException(MessageKind.Product_CannotRate_AlreadyRated);

            _ratings.Add(new Rating(Guid.NewGuid(), value, user, comment));
            RefreshRatings();
        }

        public void SetReturnable(Returnable returnable)
        {
            if (Returnable != null && Returnable.Id == returnable?.Id)
                return;

            Returnable = returnable;
        }

        public void SetConditioning(ConditioningKind conditioning, decimal quantity, UnitKind unit = UnitKind.NotSpecified)
        {
            if (conditioning == ConditioningKind.Not_Specified)
                throw new ValidationException(MessageKind.Product_Conditioning_Required);

            if(conditioning != ConditioningKind.Bulk)
                unit = UnitKind.NotSpecified;

            if (conditioning == ConditioningKind.Bouquet || conditioning == ConditioningKind.Bunch)
                quantity = 1;

            if (quantity <= 0)
                throw new ValidationException(MessageKind.Product_QuantityPerUnit_CannotBe_LowerOrEqualThan, 0);

            if (conditioning == ConditioningKind.Bulk && unit == UnitKind.NotSpecified)
                throw new ValidationException(MessageKind.Product_BulkConditioning_Requires_Unit_ToBe_Specified);

            Conditioning = conditioning;
            QuantityPerUnit = quantity;
            Unit = unit;

            RefreshPrices();
        }
        
        public void AddPicture(ProductPicture picture)
        {
            if (Pictures == null)
                _pictures = new List<ProductPicture>();
            
            _pictures.Add(picture);
        }
        
        public void RemovePicture(Guid id)
        {
            if (Pictures == null || !Pictures.Any())
                throw SheaftException.NotFound();

            var existingPicture = Pictures.FirstOrDefault(p => p.Id == id);
            if (existingPicture == null)
                throw SheaftException.NotFound();
            
            _pictures.Remove(existingPicture);
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
                _tags = new List<ProductTag>();
            
            foreach (var tag in tags)
                _tags.Add(new ProductTag(tag));
        }

        private void RefreshRatings()
        {
            Rating = Ratings.Any() ? Ratings.Average(r => r.Value) : (decimal?)null;
            RatingsCount = Ratings.Count;
        }

        public void AddOrUpdateCatalogPrice(Catalog catalog, decimal wholeSalePricePerUnit)
        {
            if (CatalogsPrices == null)
                _catalogsPrices = new List<CatalogProduct>();

            try
            {
                var existingCatalogPrice = _catalogsPrices.SingleOrDefault(c => c.Catalog.Id == catalog.Id);
                if (existingCatalogPrice == null)
                    _catalogsPrices.Add(new CatalogProduct(this, catalog, wholeSalePricePerUnit));
                else
                    existingCatalogPrice.SetWholeSalePricePerUnit(wholeSalePricePerUnit);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void RemoveFromCatalog(Guid catalogId)
        {
            if (CatalogsPrices == null || !CatalogsPrices.Any())
                throw SheaftException.NotFound();
            
            var existingCatalogPrice = _catalogsPrices.SingleOrDefault(c => c.Catalog.Id == catalogId);
            if (existingCatalogPrice == null)
                throw SheaftException.NotFound();
            
            _catalogsPrices.Remove(existingCatalogPrice);
        }

        public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();
    }
}
