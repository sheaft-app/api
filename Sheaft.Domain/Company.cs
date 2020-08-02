using Sheaft.Domains.Extensions;
using Sheaft.Exceptions;
using Sheaft.Interop;
using Sheaft.Interop.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sheaft.Domain.Models
{
    public class Company : IEntity
    {
        private List<CompanyTag> _tags;
        private List<TimeSlotHour> _openingHours;

        protected Company()
        {
        }

        public Company(Guid id, string name, ProfileKind kind, string email, string siret, string vatIdentifier, Address address, IEnumerable<TimeSlotHour> openingHours = null, bool appearInBusinessSearchResults = true, string phone = null, string description = null)
        {
            if (address == null)
                throw new ValidationException(MessageKind.Company_Address_Required);

            Id = id;
            Kind = kind;

            SetAppearInBusinessSearchResults(appearInBusinessSearchResults);
            SetEmail(email);
            SetName(name);
            SetPhone(phone);
            SetSiret(siret);
            SetVatIdentifier(vatIdentifier);
            SetDescription(description);
            SetAddress(address.Line1, address.Line2, address.Zipcode, address.City, address.Department, address.Longitude, address.Latitude);
            SetOpeningHours(openingHours);
        }

        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public ProfileKind Kind { get; private set; }
        public string Name { get; private set; }
        public bool AppearInBusinessSearchResults { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public string Reason { get; private set; }
        public string Picture { get; private set; }
        public string Description { get; private set; }
        public string VatIdentifier { get; private set; }
        public string Siret { get; private set; }
        public virtual Address Address { get; private set; }
        public virtual IReadOnlyCollection<CompanyTag> Tags { get { return _tags.AsReadOnly(); } }
        public virtual IReadOnlyCollection<TimeSlotHour> OpeningHours { get { return _openingHours.AsReadOnly(); } }

        public void SetOpeningHours(IEnumerable<TimeSlotHour> openingHours)
        {
            _openingHours = openingHours.ToList();
        }

        public void SetAddress(string line1, string line2, string zipcode, string city, Department department, double? longitude = null, double? latitude = null)
        {
            Address = new Address(line1, line2, zipcode, city, department, longitude, latitude);
        }

        public void SetName(string name)
        {
            if (name.IsNotNullAndIsEmptyOrWhiteSpace())
                throw new ValidationException(MessageKind.Company_Name_Required);

            Name = name;
        }

        public void SetPicture(string imageUrl)
        {
            Picture = imageUrl;
        }

        public void SetAppearInBusinessSearchResults(bool appearInBusinessSearchResults)
        {
            AppearInBusinessSearchResults = appearInBusinessSearchResults;
        }

        public void SetEmail(string email)
        {
            if (email.IsNotNullAndIsEmptyOrWhiteSpace())
                throw new ValidationException(MessageKind.Company_Email_Required);

            Email = email;
        }

        public void SetSiret(string siret)
        {
            if (siret.IsNotNullAndIsEmptyOrWhiteSpace())
                throw new ValidationException(MessageKind.Company_Siret_Required);

            Siret = siret;
        }

        public void SetPhone(string phone)
        {
            if (phone == null)
                return;

            Phone = phone;
        }

        public void SetDescription(string description)
        {
            if (description == null)
                return;

            Description = description;
        }

        public void SetVatIdentifier(string vatNumber)
        {
            if (vatNumber.IsNotNullAndIsEmptyOrWhiteSpace())
                throw new ValidationException(MessageKind.Company_Vat_Required);

            VatIdentifier = vatNumber;
        }

        public void SetTags(IEnumerable<Tag> tags)
        {
            if (!Tags.Any())
                _tags = new List<CompanyTag>();

            _tags.Clear();

            if (tags != null && tags.Any())
                AddTags(tags);
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
            _tags.Add(new CompanyTag(tag));
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

        public void CloseCompany(string reason)
        {
            Email = string.Empty;
            Phone = string.Empty;
            Reason = reason;
            SetAddress("", "", Address.Zipcode, "", null, null);
            RemovedOn = DateTime.UtcNow;
        }
    }
}