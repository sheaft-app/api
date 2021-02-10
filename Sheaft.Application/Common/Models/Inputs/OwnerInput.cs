﻿using System;
using Sheaft.Domain.Enum;

namespace Sheaft.Application.Common.Models.Inputs
{
    public class OwnerInput
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public CountryIsoCode Nationality { get; set; }
        public CountryIsoCode CountryOfResidence { get; set; }
        public AddressInput Address { get; set; }
    }
}