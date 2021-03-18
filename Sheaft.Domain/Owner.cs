using System;
using Sheaft.Core.Enums;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class Owner : IIdEntity
    {
        protected Owner()
        {
        }

        public Owner(Guid id, string firstname, string lastname, string email, DateTimeOffset birthdate, OwnerAddress address, CountryIsoCode nationality, CountryIsoCode countryOfResidence)
        {
            Id = id;
            SetFirstname(firstname);
            SetLastname(lastname);
            SetEmail(email);
            SetBirthDate(birthdate);
            SetAddress(address);
            SetNationality(nationality);
            SetCountryOfResidence(countryOfResidence);
        }

        public Guid Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public DateTimeOffset BirthDate { get; private set; }
        public CountryIsoCode Nationality { get; private set; }
        public CountryIsoCode CountryOfResidence { get; private set; }
        public virtual OwnerAddress Address { get; private set; }

        public void SetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ValidationException(MessageKind.Owner_Email_Required);

            Email = email;
        }

        public void SetFirstname(string firstName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ValidationException(MessageKind.Owner_Firstname_Required);

            FirstName = firstName;
        }

        public void SetLastname(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ValidationException(MessageKind.Owner_Lastname_Required);

            LastName = lastName;
        }

        public void SetBirthDate(DateTimeOffset birthdate)
        {
            if (birthdate.Year < 1900)
                throw new ValidationException(MessageKind.Owner_Birthdate_Required);

            BirthDate = birthdate;
        }

        public void SetCountryOfResidence(CountryIsoCode countryOfResidence)
        {
            if (countryOfResidence == CountryIsoCode.NotSpecified)
                throw new ValidationException(MessageKind.Owner_CountryOfResidence_Required);

            CountryOfResidence = countryOfResidence;
        }

        public void SetNationality(CountryIsoCode nationality)
        {
            if (nationality == CountryIsoCode.NotSpecified)
                throw new ValidationException(MessageKind.Owner_Nationality_Required);

            Nationality = nationality;
        }

        public void SetAddress(OwnerAddress address)
        {
            if (address == null)
                throw new ValidationException(MessageKind.Owner_Address_Required);

            Address = address;
        }
    }
}