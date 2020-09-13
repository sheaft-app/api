using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Domain.Models
{
    public class Ubo
    {
        protected Ubo() { }

        public Ubo(Guid id, string firstname, string lastname, UboAddress address, BirthAddress birthAddress, CountryIsoCode nationality)
        {
            Id = id;
            FirstName = firstname;
            LastName = lastname;
            Birthdate = Birthdate;
            BirthAddress = birthAddress;
            Address = address;
            Nationality = nationality;
        }

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset Birthdate { get; set; }
        public CountryIsoCode Nationality { get; set; }
        public virtual UboAddress Address { get; set; }
        public virtual BirthAddress BirthAddress { get; set; }
    }
}