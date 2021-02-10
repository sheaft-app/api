using System;
using System.Collections.Generic;
using System.Linq;
using Sheaft.Domain.Common;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Exceptions;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class Product : IEntity, IHasDomainEvent
    {
        private const int DESCRIPTION_MAXLENGTH = 500;
        private const int DIGITS_COUNT = 2;

        private List<ProductTag> _tags;
        private List<Rating> _ratings;

        protected Product()
        {
        }

        public Product(Guid id, string reference, string name, decimal price, ConditioningKind conditioning, UnitKind unit, decimal quantityPerUnit, Producer producer)
        {
            Id = id;
            Producer = producer ?? throw new ValidationException(MessageKind.Product_Producer_Required);

            SetName(name);
            SetReference(reference);
            SetConditioning(conditioning, quantityPerUnit, unit);
            SetWholeSalePricePerUnit(price);

            _tags = new List<ProductTag>();
            _ratings = new List<Rating>();
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
        public decimal WholeSalePricePerUnit { get; private set; }
        public decimal VatPricePerUnit { get; private set; }
        public decimal OnSalePricePerUnit { get; private set; }
        public string Description { get; private set; }
        public string Picture { get; private set; }
        public UnitKind Unit { get; private set; }
        public decimal QuantityPerUnit { get; private set; }
        public ConditioningKind Conditioning { get; private set; }
        public decimal OnSalePrice { get; set; }
        public decimal WholeSalePrice { get; set; }
        public decimal VatPrice { get; set; }
        public decimal Vat { get; private set; }
        public bool Available { get; private set; }
        public bool VisibleToConsumers { get; private set; }
        public bool VisibleToStores { get; private set; }
        public int RatingsCount { get; set; }
        public decimal? Rating { get; set; }
        public virtual Returnable Returnable { get; private set; }
        public virtual Producer Producer { get; private set; }
        public virtual IReadOnlyCollection<ProductTag> Tags => _tags?.AsReadOnly();
        public virtual IReadOnlyCollection<Rating> Ratings => _ratings?.AsReadOnly();

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

        public void SetStoreVisibility(bool? visible)
        {
            VisibleToStores = visible ?? VisibleToStores;
        }

        public void SetConsumerVisibility(bool? visible)
        {
            VisibleToConsumers = visible ?? VisibleToConsumers;
        }

        public void SetTags(IEnumerable<Tag> tags)
        {
            if(!Tags.Any())
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

        public void SetWholeSalePricePerUnit(decimal newPrice)
        {
            if (newPrice <= 0)
                throw new ValidationException(MessageKind.Product_WholeSalePrice_CannotBe_LowerOrEqualThan, 0);

            WholeSalePricePerUnit = Math.Round(newPrice, DIGITS_COUNT);
            RefreshPrices();
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

        public void RemoveRatings(IEnumerable<Guid> ids)
        {
            foreach (var id in ids)
            {
                RemoveRating(id);
            }
        }

        public void RemoveRating(Guid id)
        {
            var rating = _ratings.SingleOrDefault(r => r.Id == id);
            _ratings.Remove(rating);

            RefreshRatings();
        }

        public void AddTags(IEnumerable<Tag> tags)
        {
            foreach (var tag in tags)
            {
                AddTag(tag);
            }
        }

        public void AddTag(Tag tag)
        {
            _tags.Add(new ProductTag(tag));
        }

        public void RemoveTags(IEnumerable<Guid> ids)
        {
            foreach (var id in ids)
            {
                RemoveTag(id);
            }
        }

        public void RemoveTag(Guid id)
        {
            var tag = _tags.SingleOrDefault(r => r.Tag.Id == id);
            _tags.Remove(tag);
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

        protected void RefreshPrices()
        {
            VatPricePerUnit = Math.Round(WholeSalePricePerUnit * Vat / 100, DIGITS_COUNT);
            OnSalePricePerUnit = Math.Round(WholeSalePricePerUnit + VatPricePerUnit, DIGITS_COUNT);

            switch (Unit)
            {
                case UnitKind.ml:
                    WholeSalePrice = Math.Round((WholeSalePricePerUnit / QuantityPerUnit) * 1000, DIGITS_COUNT);
                    break;
                case UnitKind.l:
                    WholeSalePrice = Math.Round(WholeSalePricePerUnit / QuantityPerUnit, DIGITS_COUNT);
                    break;
                case UnitKind.g:
                    WholeSalePrice = Math.Round((WholeSalePricePerUnit / QuantityPerUnit) * 1000, DIGITS_COUNT);
                    break;
                case UnitKind.kg:
                    WholeSalePrice = Math.Round(WholeSalePricePerUnit / QuantityPerUnit, DIGITS_COUNT);
                    break;
                default:
                    WholeSalePrice = WholeSalePricePerUnit;
                    break;
            }

            VatPrice = Math.Round(WholeSalePrice * Vat / 100, DIGITS_COUNT);
            OnSalePrice = Math.Round(WholeSalePrice + VatPrice, DIGITS_COUNT);
        }

        private void RefreshRatings()
        {
            Rating = Ratings.Any() ? Ratings.Average(r => r.Value) : (decimal?)null;
            RatingsCount = Ratings.Count;
        }

        public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();
    }
}