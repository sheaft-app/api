using Sheaft.Interop.Enums;
using System.Collections.Generic;

namespace Sheaft.Models.Dto
{
    public class LegalProfileDto
    {
        public LegalKind Kind { get; set; }
        public OwnerDto Owner { get; set; }
    }

    public class ConsumerLegalDto : LegalProfileDto
    {

    }

    public class BusinessLegalDto : LegalProfileDto
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Identifier { get; set; }
        public AddressDto Address { get; set; }
        public IEnumerable<UboDto> Ubos { get; set; }
    }
}