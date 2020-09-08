using Sheaft.Interop.Enums;
using System;
using System.Collections.Generic;

namespace Sheaft.Models.Dto
{
    public class ProducerDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Picture { get; set; }
        public string Description { get; set; }
        public string VatIdentifier { get; set; }
        public string Siret { get; set; }
        public bool OpenForNewBusiness { get; set; }
        public OwnerDto Owner { get; set; }
        public AddressDto Address { get; set; }
        public AddressDto BillingAddress { get; set; }
        public IEnumerable<TagDto> Tags { get; set; }
    }
}