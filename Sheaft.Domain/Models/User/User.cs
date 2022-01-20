using System;
using System.Collections.Generic;
using Sheaft.Domain.Common;
using Sheaft.Domain.Enum;
using Sheaft.Domain.Events;
using Sheaft.Domain.Interop;

namespace Sheaft.Domain
{
    public class User : IEntity, IHasDomainEvent
    {
        protected User()
        {
        }

        internal User(Guid id, string name, string firstname, string lastname, string email, string phone = null)
        {
            Id = id;
        }

        public Guid Id { get; private set; }
        public DateTimeOffset CreatedOn { get; private set; }
        public DateTimeOffset UpdatedOn { get; private set; }
        public bool Removed { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public string Picture { get; private set; }
        public string Firstname { get; private set; }
        public string Lastname { get; private set; }
        public Guid? CompanyId { get; private set; }
        public UserAddress Address { get; private set; }
        public ICollection<Cart> Carts { get; private set; }
        public List<DomainEvent> DomainEvents { get; private set; } = new List<DomainEvent>();

        public void SetAddress(string line1, string line2, string zipcode, string city, CountryIsoCode country, double? longitude = null, double? latitude = null)
        {
            Address = new UserAddress(line1, line2, zipcode, city, country, longitude, latitude);
        }

        public void Remove()
        {
            Firstname = string.Empty;
            Lastname = string.Empty;
            Email = $"{Guid.NewGuid():N}@anonym.ous";
            Phone = string.Empty;
            SetAddress("Anonymous", null, Address.Zipcode, "Anonymous", Address.Country);
        }

        public void Restore()
        {
            Removed = false;
        }
    }
}