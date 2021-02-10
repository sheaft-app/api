using Sheaft.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Sheaft.Application.Models
{

    public class BirthAddressViewModel
    {
        public string City { get; set; }
        public CountryIsoCode Country { get; set; }
    }
}
