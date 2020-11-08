using Sheaft.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{
    public class ProducerDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Picture { get; set; }
        public string Description { get; set; }
        public string VatIdentifier { get; set; }
        public string Siret { get; set; }
        public bool OpenForNewBusiness { get; set; }
        public bool NotSubjectToVat { get; set; }
        public AddressDto Address { get; set; }
        public IEnumerable<TagDto> Tags { get; set; }
    }
}