using System;
using System.Collections.Generic;

namespace Sheaft.Models.Inputs
{
    public class BusinessInput
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Description { get; set; }
        public string VatIdentifier { get; set; }
        public string Picture { get; set; }
        public string Siret { get; set; }
        public FullAddressInput Address { get; set; }
        public bool OpenForNewBusiness { get; set; }
        public IEnumerable<Guid> Tags { get; set; }

    }
}