using Sheaft.Interop;
using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Domain.Models
{
    public class Owner : IIdEntity
    {
        protected Owner()
        {
        }

        public Owner(Guid id, string firstname, string lastname, string email, DateTimeOffset birthdate, OwnerAddress address, CountryIsoCode nationality, CountryIsoCode countryOfResidence)
        {
            Id = id;
            Email = email;
            FirstName = firstname;
            LastName = lastname;
            Birthdate = birthdate;
            Address = address;
            Nationality = nationality;
            CountryOfResidence = countryOfResidence;
        }

        public Guid Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public DateTimeOffset Birthdate { get; private set; }
        public CountryIsoCode Nationality { get; private set; }
        public CountryIsoCode CountryOfResidence { get; private set; }
        public virtual OwnerAddress Address { get; private set; }

        public void SetEmail(string email)
        {
            if (email == null)
                return;

            Email = email;
        }
    }
}