using System;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class Ubo: IIdEntity, ITrackCreation, ITrackUpdate
    {
        protected Ubo() { }

        public Ubo(Guid id, string firstname, string lastname, DateTimeOffset birthdate, UboAddress address, BirthAddress birthAddress, CountryIsoCode nationality)
        {
            Id = id;
            FirstName = firstname;
            LastName = lastname;
            BirthDate = birthdate;
            BirthPlace = birthAddress;
            Address = address;
            Nationality = nationality;
        }

        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset? UpdatedOn { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Identifier { get; private set; }
        public DateTimeOffset BirthDate { get; private set; }
        public CountryIsoCode Nationality { get; private set; }
        public Guid DeclarationId { get; set; }
        public UboAddress Address { get; private set; }
        public BirthAddress BirthPlace { get; private set; }

        public void SetIdentifier(string identifier)
        {
            Identifier = identifier;
        }

        public void SetFirstName(string firstname)
        {
            FirstName = firstname;
        }

        public void SetLastName(string lastname)
        {
            LastName = lastname;
        }

        public void SetBirthDate(DateTimeOffset birthdate)
        {
            BirthDate = birthdate;
        }

        public void SetNationality(CountryIsoCode nationality)
        {
            Nationality = nationality;
        }

        public void SetAddress(UboAddress address)
        {
            Address = address;
        }

        public void SetBirthPlace(BirthAddress birthPlace)
        {
            BirthPlace = birthPlace;
        }
    }
}