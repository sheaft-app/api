﻿using System;
using Sheaft.Models.Inputs;
using Sheaft.Core;
using Newtonsoft.Json;
using Sheaft.Interop.Enums;

namespace Sheaft.Application.Commands
{
    public class CreateConsumerLegalCommand : Command<Guid>
    {
        [JsonConstructor]
        public CreateConsumerLegalCommand(RequestUser requestUser) : base(requestUser)
        {
        }

        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public CountryIsoCode Nationality { get; set; }
        public CountryIsoCode CountryOfResidence { get; set; }
        public AddressInput Address { get; set; }
    }
}