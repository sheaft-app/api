using System;
using Sheaft.Core.Exceptions;
using Sheaft.Domain.Enum;

namespace Sheaft.Domain
{
    public class Owner
    {
        protected Owner()
        {
        }

        public Owner(string firstname, string lastname, string email, DateTimeOffset birthdate, OwnerAddress address, CountryIsoCode nationality, CountryIsoCode countryOfResidence)
        {
            SetFirstname(firstname);
            SetLastname(lastname);
            SetEmail(email);
            SetBirthDate(birthdate);
            SetAddress(address);
            SetNationality(nationality);
            SetCountryOfResidence(countryOfResidence);
        }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public DateTimeOffset BirthDate { get; private set; }
        public CountryIsoCode Nationality { get; private set; }
        public CountryIsoCode CountryOfResidence { get; private set; }
        public OwnerAddress Address { get; private set; }
        public byte[] RowVersion { get; private set; }

        public void SetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw SheaftException.Validation("L'email du représentant légal est requis.");

            Email = email;
        }

        public void SetFirstname(string firstName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw SheaftException.Validation("Le prénom du représentant légal est requis.");

            FirstName = firstName;
        }

        public void SetLastname(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
                throw SheaftException.Validation("Le nom du représentant légal est requis.");

            LastName = lastName;
        }

        public void SetBirthDate(DateTimeOffset birthdate)
        {
            if (birthdate.Year < 1900)
                throw SheaftException.Validation("La date de naissance du représentant légal est requise.");

            BirthDate = birthdate;
        }

        public void SetCountryOfResidence(CountryIsoCode countryOfResidence)
        {
            if (countryOfResidence == CountryIsoCode.NotSpecified)
                throw SheaftException.Validation("Le pays de résidence du représentant légal est requis.");

            CountryOfResidence = countryOfResidence;
        }

        public void SetNationality(CountryIsoCode nationality)
        {
            if (nationality == CountryIsoCode.NotSpecified)
                throw SheaftException.Validation("La nationalité du représentant légal est requise.");

            Nationality = nationality;
        }

        public void SetAddress(OwnerAddress address)
        {
            if (address == null)
                throw SheaftException.Validation("L'adresse du représentant légal est requise.");

            Address = address;
        }
    }
}