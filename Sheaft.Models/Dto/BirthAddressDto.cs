using Sheaft.Interop.Enums;
using System;
using System.Collections.Generic;

namespace Sheaft.Models.Dto
{
    public class BirthAddressDto
    {
        public string City { get; set; }
        public CountryIsoCode Country { get; set; }
    }
}