using Sheaft.Domains.Extensions;
using Sheaft.Exceptions;
using Sheaft.Domain.Enums;
using Sheaft.Domain.Interop;
using System;

namespace Sheaft.Domain.Models
{
    public abstract class User : IEntity
    {
        protected User()
        {
        }

        protected User(Guid id, ProfileKind kind, string name, string firstname, string lastname, string email, string phone = null)
        {
            Id = id;

            SetProfileKind(kind);
            SetUserName(name);
            SetEmail(email);
            SetPhone(phone);
            SetFirstname(firstname);
            SetLastname(lastname);
        }

        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public DateTimeOffset? RemovedOn { get; private set; }
        public string Identifier { get; private set; }
        public ProfileKind Kind { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public string Picture { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string SponsorshipCode { get; private set; }
        public int TotalPoints { get; private set; }
        public virtual UserAddress Address { get; private set; }

        public void SetFirstname(string firstname)
        {
            if (string.IsNullOrWhiteSpace(firstname))
                throw new ValidationException(MessageKind.Consumer_Firstname_Required);

            FirstName = firstname;

            if (Kind == ProfileKind.Consumer)
                SetUserName($"{FirstName} {LastName}");
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
            Address = new UserAddress(department);
        }

        public void SetAddress(UserAddress address)
        {
            Address = address;
        }

        public void SetAddress(string line1, string line2, string zipcode, string city, CountryIsoCode country, Department department, double? longitude = null, double? latitude = null)
        {
            Address = new UserAddress(line1, line2, zipcode, city, country, department, longitude, latitude);
        }

        protected void SetUserName(string name)
        {
            if (name.IsNotNullAndIsEmptyOrWhiteSpace())
                throw new ValidationException(MessageKind.Business_Name_Required);

            Name = name;
        }

        public void SetProfileKind(ProfileKind? kind)
        {
            if (!kind.HasValue)
                return;

            Kind = kind.Value;
        }

        public void SetEmail(string email)
        {
            if (email.IsNotNullAndIsEmptyOrWhiteSpace())
                throw new ValidationException(MessageKind.Business_Email_Required);

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
            SponsorshipCode = code;
        }

        public void SetIdentifier(string identifier)
        {
            if (identifier == null)
                return;

            Identifier = identifier;
        }

        public virtual void Close(string reason)
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = $"{Guid.NewGuid():N}@a.c";
            Phone = string.Empty;
            RemovedOn = DateTime.UtcNow;
            SetAddress("", "", Address.Zipcode, "", Address.Country, Address.Department);
        }

        public void SetTotalPoints(int points)
        {
            TotalPoints = points;
        }
    }

    public class Admin : User
    {
        protected Admin()
        {
        }

        public Admin(Guid id, string name, string firstname, string lastname, string email)
            : base(id, ProfileKind.Admin, name, firstname, lastname, email) { }
    }

    public class Support : User
    {
        protected Support()
        {
        }
        public Support(Guid id, string name, string firstname, string lastname, string email)
            : base(id, ProfileKind.Support, name, firstname, lastname, email) { }
    }
}