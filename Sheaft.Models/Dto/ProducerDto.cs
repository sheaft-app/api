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
        public AddressDto Address { get; set; }
        public IEnumerable<TagDto> Tags { get; set; }
    }
}