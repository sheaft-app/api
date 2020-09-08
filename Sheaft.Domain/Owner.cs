using Sheaft.Interop.Enums;
using System;

namespace Sheaft.Domain.Models
{
    public class Owner
    {
        protected Owner()
        {
        }

        public Owner(OwnerKind kind, string nationality, string line1, string zipcode, string city, string country, DateTimeOffset? birthdate = null, string line2 = null)
        {
            Kind = kind;
            Birthdate = birthdate;
            Nationality = nationality;
            Line1 = line1;
            Line2 = line2;
            Zipcode = zipcode;
            City = city;
            Country = country;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset? Birthdate { get; set; }
        public OwnerKind Kind { get; set; }
        public string Nationality { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Zipcode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}