using System.Collections.Generic;

namespace Sheaft.Models.Dto
{
    public class BusinessProfileDto : UserDto
    {
        public string Description { get; set; }
        public string Siret { get; set; }
        public string VatIdentifier { get; set; }
    }
}