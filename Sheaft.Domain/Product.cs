using Sheaft.Exceptions;
using Sheaft.Interop;
using Sheaft.Interop.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sheaft.Domain.Models
{
    public class Product : IEntity
    {
        private const int DESCRIPTION_MAXLENGTH = 500;
        private const int DIGITS_COUNT = 2;

        private List<ProductTag> _tags;
        private List<Rating> _ratings;

        protected Product()
        {
        }

        public Product(Guid id, string reference, string name, decimal wholeSalePricePerUnit, UnitKind unit, decimal quantityPerUnit, decimal vat, Producer producer)
        {
            if (producer == null)
                throw new ValidationException(MessageKind.Product_Producer_Required);

            Id = id;
            Producer = producer;

            SetName(name);
            SetReference(reference);
            SetUnit(quantityPerUnit, unit);
            SetWholeSalePricePerUnit(wholeSalePricePerUnit);
            SetVat(vat);

            _tags = new List<ProductTag>();
            _ratings = new List<Rating>();

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
        public string Image { get; private set; }
        public decimal QuantityPerUnit { get; private set; }
        public UnitKind Unit { get; private set; }
        public decimal OnSalePrice { get; set; }
        public decimal WholeSalePrice { get; set; }
        public decimal VatPrice { get; set; }
        public decimal Vat { get; private set; }
        public bool Available { get; private set; }
        public int RatingsCount { get; set; }
        public decimal? Rating { get; set; }
        public virtual Packaging Packaging { get; private set; }
        public virtual Producer Producer { get; private set; }
        public virtual IReadOnlyCollection<ProductTag> Tags { get { return _tags.AsReadOnly(); } }
        public virtual IReadOnlyCollection<Rating> Ratings { get { return _ratings.AsReadOnly(); } }

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
            if(!Tags.Any())
                _tags = new List<ProductTag>();

            _tags.Clear();

            if (tags != null && tags.Any())
                AddTags(tags);
        }

        public void SetImage(string image)
        {
            if (image == null)
                return;

            Image = image;
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

        public void SetVat(decimal newVat)
        {
            if (newVat < 0)
                throw new ValidationException(MessageKind.Product_Vat_CannotBe_LowerThan, 0);

            if (newVat > 100)
                throw new ValidationException(MessageKind.Product_Vat_CannotBe_GreaterThan, 100);

            Vat = newVat;
            VatPricePerUnit = Math.Round(Vat, DIGITS_COUNT);
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

        public void SetPackaging(Packaging packaging)
        {
            if (Packaging != null && Packaging.Id == packaging?.Id)
                return;

            Packaging = packaging;
        }

        public void SetUnit(decimal quantityPerUnit, UnitKind unit)
        {
            if (quantityPerUnit <= 0)
                throw new ValidationException(MessageKind.Product_QuantityPerUnit_CannotBe_LowerOrEqualThan, 0);

            QuantityPerUnit = quantityPerUnit;
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
                    WholeSalePrice = Math.Round((WholeSalePricePerUnit / QuantityPerUnit), DIGITS_COUNT);
                    break;
            }

            VatPrice = Math.Round(WholeSalePrice * Vat / 100, DIGITS_COUNT);
            OnSalePrice = Math.Round(WholeSalePrice + VatPrice, DIGITS_COUNT);
        }

        protected void RefreshRatings()
        {
            Rating = Ratings.Any() ? Ratings.Average(r => r.Value) : (decimal?)null;
            RatingsCount = Ratings.Count();
        }
    }
}