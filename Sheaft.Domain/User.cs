using Sheaft.Domains.Extensions;
using Sheaft.Exceptions;
using Sheaft.Interop;
using Sheaft.Interop.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sheaft.Domain.Models
{
    public abstract class User : IEntity
    {
        private List<Points> _points;
        private List<PaymentMethod> _paymentMethods;
        private List<Wallet> _wallets;
        private List<Document> _documents;

        protected User()
        {
        }

        protected User(Guid id, ProfileKind kind, string name, string firstname, string lastname, string email, string phone = null)
        {
            Id = id;

            SetKind(kind);
            SetUserName(name);
            SetEmail(email);
            SetPhone(phone);
            SetFirstname(firstname);
            SetLastname(lastname);

            _points = new List<Points>();
            _paymentMethods = new List<PaymentMethod>();
            _wallets = new List<Wallet>();
            _documents = new List<Document>();

            RefreshPoints();
        }

        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public string Identifier { get; private set; }
        public ProfileKind Kind { get; private set; }
        public string Reason { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public string Picture { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public DateTimeOffset? Birthdate { get; set; }
        public string Nationality { get; set; }
        public string CountryOfResidence { get; set; }
        public string Code { get; private set; }
        public int TotalPoints { get; private set; }
        public virtual Address Address { get; private set; }
        public virtual BillingAddress BillingAddress { get; private set; }
        public virtual IReadOnlyCollection<Points> Points { get { return _points.AsReadOnly(); } }
        public virtual IReadOnlyCollection<PaymentMethod> PaymentMethods { get { return _paymentMethods.AsReadOnly(); } }
        public virtual IReadOnlyCollection<Wallet> Wallets { get { return _wallets.AsReadOnly(); } }
        public virtual IReadOnlyCollection<Document> Documents { get { return _documents.AsReadOnly(); } }

        public void SetFirstname(string firstname)
        {
            if (string.IsNullOrWhiteSpace(firstname))
                throw new ValidationException(MessageKind.Consumer_Firstname_Required);

            FirstName = firstname;

            if(Kind == ProfileKind.Consumer)
                SetUserName($"{FirstName} {LastName}");
        }

        public void SetNationality(string nationality)
        {
            if (nationality == null)
                return;

            Nationality = nationality;
        }

        public void SetCountryOfResidence(string country)
        {
            if (country == null)
                return;

            CountryOfResidence = country;
        }

        public void SetLastname(string lastname)
        {
            if (string.IsNullOrWhiteSpace(lastname))
                throw new ValidationException(MessageKind.Consumer_Lastname_Required);

            LastName = lastname;

            if (Kind == ProfileKind.Consumer)
                SetUserName($"{FirstName} {LastName}");
        }

        public void SetAddress(Department department)
        {
            Address = new Address(department);
        }
        public void SetAddress(string line1, string line2, string zipcode, string city, Department department, double? longitude = null, double? latitude = null)
        {
            Address = new Address(line1, line2, zipcode, city, department, longitude, latitude);
        }
        public void SetBillingAddress(string line1, string line2, string zipcode, string city)
        {
            BillingAddress = new BillingAddress(line1, line2, zipcode, city);
        }

        protected void SetUserName(string name)
        {
            if (name.IsNotNullAndIsEmptyOrWhiteSpace())
                throw new ValidationException(MessageKind.Company_Name_Required);

            Name = name;
        }

        public virtual void Close(string reason)
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = $"{Guid.NewGuid():N}@a.c";
            Phone = string.Empty;
            Reason = reason;
            RemovedOn = DateTime.UtcNow;
        }

        public void SetKind(ProfileKind? kind)
        {
            if (!kind.HasValue)
                return;

            Kind = kind.Value;
        }

        public void SetEmail(string email)
        {
            if (email.IsNotNullAndIsEmptyOrWhiteSpace())
                throw new ValidationException(MessageKind.Company_Email_Required);

            Email = email;
        }
        public void SetPhone(string phone)
        {
            if (phone == null)
                return;

            Phone = phone;
        }

        public void SetPicture(string picture)
        {
            if (picture == null)
                return;

            Picture = picture;
        }
        
        public void SetSponsoringCode(string code)
        {
            Code = code;
        }

        public void AddPoints(PointKind kind, int quantity, DateTimeOffset? createdOn = null)
        {
            if (Points == null)
                _points = new List<Points>();

            _points.Add(new Points(kind, quantity, createdOn ?? DateTimeOffset.UtcNow));
            RefreshPoints();
        }

        public void Remove()
        {
        }

        public void Restore()
        {
            RemovedOn = null;
        }

        private void RefreshPoints()
        {
            TotalPoints = _points.Sum(c => c.Quantity);
        }
    }
}